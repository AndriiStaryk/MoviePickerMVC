using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Services;

public class DirectorImportService : IImportService<Director>
{
    private readonly MoviePickerV2Context _context;

    public DirectorImportService(MoviePickerV2Context context)
    {
        _context = context;
    }

    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
        }

        try
        {
            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {
                    if (worksheet.RowsUsed().Count() <= 1)
                    {
                        throw new Exception("The worksheet is empty");
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        try
                        {
                            await AddDirectorAsync(row, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during import: " + ex.Message, ex);
        }
    }


    private async Task<string> AddDirectorAsync(IXLRow row, CancellationToken cancellationToken)
    {
        const int NameColumn = 1;
        const int BirthDateColumn = 2;
        const int BirthCountryColumn = 3;

        var directorName = row.Cell(NameColumn).Value.ToString();
        var directorBirthDateTimeString = row.Cell(BirthDateColumn).Value.ToString();
        var directorBirthCountryString = row.Cell(BirthCountryColumn).Value.ToString();


        if (string.IsNullOrEmpty(directorName) ||
            string.IsNullOrEmpty(directorBirthDateTimeString) ||
            string.IsNullOrEmpty(directorBirthCountryString))
        {
            throw new Exception("Some of required fields are empty.");
        }

        var directorBirthCountry = _context.Countries.FirstOrDefault(c => c.Name == directorBirthCountryString);
        if (directorBirthCountry == null)
        {
            throw new Exception($"Director: '{directorName}' have invalid birth country.");
        }

        if (!DateTime.TryParse(directorBirthDateTimeString, out DateTime directorBirthDateTime))
        {
            throw new Exception($"Director: '{directorName}' have invalid birth date format.");
        }

        DateOnly directorBirthDate = new DateOnly(directorBirthDateTime.Year, directorBirthDateTime.Month, directorBirthDateTime.Day);

        if (!await DirectorViewModel.IsDirectorExist(directorName, directorBirthDate, directorBirthCountry.Id, null, _context))
        {
            var director = new Director();
            director.Name = directorName;
            director.BirthDate = directorBirthDate;
            director.BirthCountryId = directorBirthCountry.Id;

            string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_person_image.jpg";
            if (System.IO.File.Exists(imagePath))
            {
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                director.DirectorImage = defaultImageBytes;
            }

            _context.Directors.Add(director);
            return "";
        }
        else
        {
            throw new Exception($"Director: {directorName} already exists.");
        }
    }
}