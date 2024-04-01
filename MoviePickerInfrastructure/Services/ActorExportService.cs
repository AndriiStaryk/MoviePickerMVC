using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public class ActorExportService : IExportService<Actor>
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

    private const int MoviesForActorCount = 3;

    private readonly MoviePickerV2Context _context;

    private static void WriteHeader(IXLWorksheet worksheet)
    {
        for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
        {
            worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
        }
        worksheet.Row(1).Style.Font.Bold = true;
    }

    private void WriteActor(IXLWorksheet worksheet, Actor actor, int rowIndex)
    {
        var columnIndex = 1;
        worksheet.Cell(rowIndex, columnIndex++).Value = actor.Name;
        worksheet.Cell(rowIndex, columnIndex++).Value = actor.BirthDate.ToString();

        var countryName = _context.Countries.Find(actor.BirthCountryId).Name;
        worksheet.Cell(rowIndex, columnIndex++).Value = countryName;


        var moviesWithActor = _context.MoviesActors.Where(ma => ma.ActorId == actor.Id)
                                                .Select(ma => ma.Movie)
                                                .ToList();



        foreach (var mov in moviesWithActor.Take(MoviesForActorCount))
        {
            worksheet.Cell(rowIndex, columnIndex++).Value = mov.Title;
        }
    }

    private void WriteActors(IXLWorksheet worksheet, ICollection<Actor> actors)
    {
        WriteHeader(worksheet);
        int rowIndex = 2;
        foreach (var actor in actors)
        {
            WriteActor(worksheet, actor, rowIndex);
            rowIndex++;
        }
    }

    public ActorExportService(MoviePickerV2Context context)
    {
        _context = context;
    }

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Input stream is not writable");
        }

        var actors = await _context.Actors
            .ToArrayAsync(cancellationToken);


        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Actors");

            WriteActors(worksheet, actors);

            workbook.SaveAs(stream);
        }
    }

}


