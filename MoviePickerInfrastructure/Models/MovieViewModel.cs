using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class MovieViewModel
{

    public Movie Movie { get; set; } = null!;

    public List<Genre> Genres { get; set; } = null!;

    public List<Genre> SelectedGenres { get; set; } = new List<Genre>();
    public MoviesGenre MoviesGenre { get; set; } = new MoviesGenre();

    public async void AddSelectedGenres(MoviePickerContext context)
    { 
        foreach (var genre  in SelectedGenres)
        {
            MoviesGenre moviesGenre = new MoviesGenre();
            moviesGenre.Genre = genre;
            moviesGenre.Movie = Movie;

            if (!await IsMoviesGenresExist(context, moviesGenre.MovieId, moviesGenre.GenreId))
            {
                context.Add(moviesGenre);
                await context.SaveChangesAsync();
            }
        }
    }

    private async Task<bool> IsMoviesGenresExist(MoviePickerContext context, int movieID, int genreID)
    {
        var moviesGenres = await context.MoviesGenres
            .FirstOrDefaultAsync(m => m.MovieId == movieID && m.GenreId == genreID);

        return moviesGenres != null;
    }
}
