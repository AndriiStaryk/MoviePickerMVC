using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviePickerInfrastructure.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresChartController : ControllerBase
{
    private readonly MoviePickerContext _context;

    public GenresChartController(MoviePickerContext context)
    {
        _context = context;
    }

    [HttpGet("JsonData")]

    public JsonResult JsonData()
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
}
