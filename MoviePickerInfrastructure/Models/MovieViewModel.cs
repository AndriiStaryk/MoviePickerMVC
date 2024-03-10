using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class MovieViewModel
{
    private MoviePickerContext _context;
    public Movie Movie { get; set; } = null!;

    public List<Genre> Genres { get; set; }

    //public List<Genre> SelectedGenres { get; set; } = new List<Genre>();

    public List<Review> Reviews { get; set; }

    public List<Actor> Actors { get; set; }

    public List<Language> Languages { get; set; }

    public Director Director { get; set; } = null!;
    public MovieViewModel(MoviePickerContext context, Movie movie)
    {
        _context = context;
        Movie = movie;

        Director = context.Directors
            .Find(movie.DirectorId)!;
            
        Reviews = context.Reviews
            .Where(review => review.MovieId == movie.Id).ToList();

        Actors = context.MoviesActors
            .Where(ma => ma.MovieId == movie.Id)
            .Select(ma => ma.Actor).ToList()!;

        Genres = context.MoviesGenres
            .Where(mg => mg.MovieId == movie.Id)
            .Select(mg => mg.Genre).ToList()!;

        Languages = context.MoviesLanguages
            .Where(ml => ml.MovieId == movie.Id)
            .Select(ml => ml.Language).ToList()!;
    }

    public List<Genre> GetAllAvailableGenres()
    {
        return _context.Genres.ToList();
    }

    //public void AddGenreById(int genreId)
    //{
    //    var genre = _context.Genres.FirstOrDefault(genre => genre.Id == genreId);
    //    if (genre != null)
    //    {
    //       SelectedGenres.Add(genre);
    //    }
    //}


    //public async void AddSelectedGenres()
    //{
    //    foreach (var genre in SelectedGenres)
    //    {
    //        MoviesGenre moviesGenre = new MoviesGenre { MovieId = Movie.Id, GenreId = genre.Id };

    //        if (!await IsMoviesGenresExist(moviesGenre.MovieId, moviesGenre.GenreId))
    //        {
    //            Movie.MoviesGenres.Add(moviesGenre);
    //            //_context.Add(moviesGenre);
    //            //await context.SaveChangesAsync();
    //        }
    //    }
        
    //}

    private async Task<bool> IsMoviesGenresExist(int movieID, int genreID)
    {
        var moviesGenres = await _context.MoviesGenres
            .FirstOrDefaultAsync(m => m.MovieId == movieID && m.GenreId == genreID);

        return moviesGenres != null;
    }
}
