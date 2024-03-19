using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartsController : ControllerBase
{
    private readonly MoviePickerV2Context _context;

    public ChartsController(MoviePickerV2Context context)
    {
        _context = context;
    }

    [HttpGet("JsonGenresData")]

    public JsonResult JsonGenresData()
    {
        var genres = _context.Genres.ToList();
        List<object> genreMovie = new List<object>();

        genreMovie.Add(new[] { "Жанр", "Кількість фільмів" });

        foreach (var genre in genres)
        {
            var moviesByGenreCount = _context.MoviesGenres.Where(mg => mg.GenreId == genre.Id).ToList().Count;
            genreMovie.Add(new object[] { genre.Name, moviesByGenreCount });
        }

        return new JsonResult(genreMovie);
     }


    [HttpGet("JsonActorsData")]
    public JsonResult JsonActorsData()
    {
        var actors = _context.Actors.ToList();
        List<object> actorMovie = new List<object>();

        actorMovie.Add(new[] { "Актор", "Кількість фільмів" });

        foreach (var actor in actors)
        {
            var moviesWithActorCount = _context.MoviesActors.Where(ma => ma.ActorId == actor.Id).ToList().Count;
            actorMovie.Add(new object[] { actor.Name, moviesWithActorCount });
        }

        return new JsonResult(actorMovie);
    }


    [HttpGet("JsonDirectorsData")]

    public JsonResult JsonDirectorsData()
    {
        var directors = _context.Directors.ToList();
        List<object> directorMovie = new List<object>();

        directorMovie.Add(new[] { "Режисер", "Кількість фільмів" });

        foreach (var director in directors)
        {
            var moviesByDirectorCount = _context.Movies.Where(m => m.DirectorId == director.Id).ToList().Count;
            directorMovie.Add(new object[] { director.Name, moviesByDirectorCount });
        }

        return new JsonResult(directorMovie);
    }


    [HttpGet("JsonLanguagesData")]

    public JsonResult JsonLanguagesData()
    {
        var languages = _context.Languages.ToList();
        List<object> languageMovie = new List<object>();

        languageMovie.Add(new[] { "Мова", "Кількість фільмів" });

        foreach (var language in languages)
        {
            var moviesWithLanguageCount = _context.MoviesLanguages.Where(ml => ml.LanguageId == language.Id).ToList().Count;
            languageMovie.Add(new object[] { language.Name, moviesWithLanguageCount });
        }

        return new JsonResult(languageMovie);
    }
}
