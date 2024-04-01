using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using MoviePickerInfrastructure.Models;

namespace MoviePickerInfrastructure.Services;

public class ActorImportService : IImportService<Actor>
{
    private readonly MoviePickerV2Context _context;

    public ActorImportService(MoviePickerV2Context context)
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
                    if (worksheet.RowsUsed().Count() <= 1) {
                        throw new Exception("The worksheet is empty");
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        try
                        {
                            await AddActorAsync(row, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        } catch (Exception ex)
        {
            throw new Exception("An error occurred during import: " + ex.Message, ex);
        }
       } 

    //private async Task AddMovie(string name, )

    private async Task<string> AddActorAsync(IXLRow row, CancellationToken cancellationToken)
    {
        const int NameColumn = 1;
        const int BirthDateColumn = 2;
        const int BirthCountryColumn = 3;

        var actorName = row.Cell(NameColumn).Value.ToString();
        var actorBirthDateTimeString = row.Cell(BirthDateColumn).Value.ToString();
        var actorBirthCountryString = row.Cell(BirthCountryColumn).Value.ToString();

       
        if (string.IsNullOrEmpty(actorName) &&
            string.IsNullOrEmpty(actorBirthDateTimeString) &&
            string.IsNullOrEmpty(actorBirthCountryString))
        {
            throw new Exception("Some of required fields are empty." );
        }

        var actorBirthCountry = _context.Countries.FirstOrDefault(c => c.Name == actorBirthCountryString);
        if (actorBirthCountry == null)
        {
            throw new Exception($"Actor: '{actorName}' have invalid birth country.");
        }

        if (!DateTime.TryParse(actorBirthDateTimeString, out DateTime actorBirthDateTime))
        {
            throw new Exception($"Actor: '{actorName}' have invalid birth date format.");
        }

        DateOnly actorBirthDate = new DateOnly(actorBirthDateTime.Year, actorBirthDateTime.Month, actorBirthDateTime.Day);

        if (!await ActorViewModel.IsActorExist(actorName, actorBirthDate, actorBirthCountry.Id, null, _context))
        {
            var actor = new Actor();
            actor.Name = actorName;
            actor.BirthDate = actorBirthDate;
            actor.BirthCountryId = actorBirthCountry.Id;

            string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_person_image.jpg";
            if (System.IO.File.Exists(imagePath))
            {
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                actor.ActorImage = defaultImageBytes;
            }

            _context.Actors.Add(actor);
            return "";
        }
        else
        {
            throw new Exception($"Actor: {actorName} already exists.");
        }
    }
}