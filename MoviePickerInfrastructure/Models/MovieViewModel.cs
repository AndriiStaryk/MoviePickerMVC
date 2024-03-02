using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class MovieViewModel
{
    private MoviePickerContext _context;
    public Movie Movie { get; set; } = null!;

    public List<Genre> Genres { get; set; }

    public List<Genre> SelectedGenres { get; set; } = new List<Genre>();

    public List<Review> Reviews { get; set; }

    public List<Actor> Actors { get; set; }

    public List<Language> Languages { get; set; }


    public MovieViewModel(MoviePickerContext context)
    {
        _context = context;
        Reviews = context.Reviews.ToList();
        //.Include(mr => mr.Review)
        //.Where(mr => mr.MovieId == id)
        //.Select(mr => mr.Review.Content)
        //.ToListAsync();
        //Reviews = context.MoviesReviews
        //    .SelectMany(movie => movie.MovieId == Movie.Id);
        Actors = context.Actors.ToList();
        Genres = context.Genres.ToList();
        Languages = context.Languages.ToList();
    }


    public async void AddSelectedGenres()
    {
        foreach (var genre in SelectedGenres)
        {
            MoviesGenre moviesGenre = new MoviesGenre();
            moviesGenre.Genre = genre;
            moviesGenre.Movie = Movie;

            if (!await IsMoviesGenresExist(moviesGenre.MovieId, moviesGenre.GenreId))
            {
                _context.Add(moviesGenre);
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task<bool> IsMoviesGenresExist(int movieID, int genreID)
    {
        var moviesGenres = await _context.MoviesGenres
            .FirstOrDefaultAsync(m => m.MovieId == movieID && m.GenreId == genreID);

        return moviesGenres != null;
    }
}
