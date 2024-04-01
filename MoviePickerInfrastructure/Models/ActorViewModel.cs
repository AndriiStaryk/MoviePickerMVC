using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;
using System.Runtime.CompilerServices;

namespace MoviePickerInfrastructure.Models;

public class ActorViewModel
{
    private MoviePickerV2Context _context;
    public Actor Actor { get; set; } = null!;

    public List<Movie> MoviesWithThisActor { get; set; }

    public ActorViewModel(MoviePickerV2Context context, Actor actor)
    {
        _context = context;
        Actor = actor;

        MoviesWithThisActor = context.MoviesActors
            .Where(ma => ma.ActorId == actor.Id)
            .Select(ma => ma.Movie).ToList()!;
    }


    public void DeleteActor()
    {
        var mas = _context.MoviesActors
            .Where(ma => ma.ActorId == Actor.Id).ToList();

        foreach (var ma in mas)
        {
            if (ma != null)
            {
                _context.MoviesActors.Remove(ma);
            }
        }

        _context.Actors.Remove(Actor);
        _context.SaveChanges();
    }

    static public async Task<bool> IsActorExist(string name,
                                                DateOnly birthDate,
                                                int birthCountryID, 
                                                IFormFile? actorImage,
                                                MoviePickerV2Context context)
    {
        byte[]? image = null;
        if (actorImage != null && actorImage.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await actorImage.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }

        var actor = await context.Actors.FirstOrDefaultAsync(a => a.Name == name &&
                                                                   a.BirthDate == birthDate &&
                                                                   a.BirthCountryId == birthCountryID);

        if (actor != null && image != null && actor.ActorImage.SequenceEqual(image))
        {
            return true;
        }

        return false;
    }

}
