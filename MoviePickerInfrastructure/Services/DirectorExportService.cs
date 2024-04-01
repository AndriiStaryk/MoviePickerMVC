using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public class DirectorExportService: IExportService<Director>
{
    private const string RootWorksheetName = "";

    private static readonly IReadOnlyList<string> HeaderNames =
    new string[]
    {
                "Ім'я",
                "Дата народження",
                "Країна народження",
                "Фільм 1",
                "Фільм 2",
                "Фільм 3",
    };

    private const int MoviesForDirectorCount = 3;

    private readonly MoviePickerV2Context _context;

    public DirectorExportService(MoviePickerV2Context context)
    {
        _context = context;
    }

    private static void WriteHeader(IXLWorksheet worksheet)
    {
        for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
        {
            worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
        }
        worksheet.Row(1).Style.Font.Bold = true;
    }

    private void WriteDirector(IXLWorksheet worksheet, Director director, int rowIndex)
    {
        var columnIndex = 1;
        worksheet.Cell(rowIndex, columnIndex++).Value = director.Name;
        worksheet.Cell(rowIndex, columnIndex++).Value = director.BirthDate.ToString();

        var countryName = _context.Countries.Find(director.BirthCountryId)!.Name;
        worksheet.Cell(rowIndex, columnIndex++).Value = countryName;


        var moviesByDirector = _context.Movies.Where(m => m.DirectorId == director.Id)
                                                .ToList();



        foreach (var mov in moviesByDirector.Take(MoviesForDirectorCount))
        {
            worksheet.Cell(rowIndex, columnIndex++).Value = mov.Title;
        }
    }

    private void WriteDirectors(IXLWorksheet worksheet, ICollection<Director> directors)
    {
        WriteHeader(worksheet);
        int rowIndex = 2;
        foreach (var director in directors)
        {
            WriteDirector(worksheet, director, rowIndex);
            rowIndex++;
        }
    }

   

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Input stream is not writable");
        }

        var directors = await _context.Directors
            .ToArrayAsync(cancellationToken);


        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Directors");

            WriteDirectors(worksheet, directors);

            workbook.SaveAs(stream);
        }
    }
}
