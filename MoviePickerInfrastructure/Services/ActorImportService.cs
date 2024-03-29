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

        using (XLWorkbook workBook = new XLWorkbook(stream))
        {
            //перегляд усіх листів (в даному випадку категорій книг)
            foreach (IXLWorksheet worksheet in workBook.Worksheets)
            {
                ////worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову

                //var actName = worksheet.Name;
                //var actor = await _context.Actors.FirstOrDefaultAsync(act => act.Name == actName, cancellationToken);
                //if (actor == null)
                //{
                //    actor = new Actor();
                //    actor.Name = actName;
                    
                //    //actor.Info = "from EXCEL";
                //    //додати в контекст
                //    _context.Actors.Add(actor);
                //}


                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    await AddActorAsync(row, cancellationToken);
                }


            }
        }
        await _context.SaveChangesAsync(cancellationToken);
    }


    private async Task AddActorAsync(IXLRow row, CancellationToken cancellationToken)
    {

        const int NameColumn = 1;
        const int BirthDateColumn = 2;
        const int BirthCountryColumn = 3;

        var actorName = row.Cell(NameColumn).Value.ToString();
        var actorBirthDateString = row.Cell(BirthDateColumn).Value.ToString();
        var actorBirthCountryString = row.Cell(BirthCountryColumn).Value.ToString();


        if (string.IsNullOrEmpty(actorName) &&
            string.IsNullOrEmpty(actorBirthDateString) &&
            string.IsNullOrEmpty(actorBirthCountryString))
        {
            return;
        }
       
        var actorBithCountry = _context.Countries.FirstOrDefault(c => c.Name == actorBirthCountryString);

        if (actorBithCountry == null)
        {
            return;
        }
        

            if (!DateOnly.TryParse(actorBirthDateString, out DateOnly actorBirthDate))
            {
                return;
            }

            if (!await ActorViewModel.IsActorExist(actorName, actorBirthDate, actorBithCountry.Id, null, _context))
            {
                var actor = new Actor();
                actor.Name = actorName;
                actor.BirthDate = actorBirthDate;
                actor.BirthCountryId = actorBithCountry.Id;
                
                _context.Actors.Add(actor);
            }
        }


        //if (!string.IsNullOrEmpty(actorName) &&
        //!string.IsNullOrEmpty(actorBirthDateString) &&
        //!string.IsNullOrEmpty(actorBirthCountryString))
        //{

        //    var actorBithCountryId = _context.Countries.FirstOrDefault(c => c.Name == actorBirthCountryString).Id;

        //    if (DateTime.TryParse(actorBirthDateString, out DateTime actorBirthDate)) 
        //    {

        //    }

        //    if (!await ActorViewModel.IsActorExist(actorName, actorBirthDate, actorBithCountryId, null, _context))
        //    {
        //        actor = new Actor();
        //        actor.Name = actName;
        //        //actor.BirthDate = row.Cell(BirthDateColumn).Value;
        //        string countryName = row.Cell(BirthCountryColumn).Value.ToString();
        //        _context.Actors.Add(actor);
        //    }
        //}

    
}