using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class MovieViewModel
{
    private MoviePickerV2Context _context = new MoviePickerV2Context();
    public Movie Movie { get; set; } = null!;

    public List<Genre> Genres { get; set; } 

    public List<Genre> AllGenres { get; set; }

    public List<Review> Reviews { get; set; } 

    public List<Actor> Actors { get; set; } 

    public List<Language> Languages { get; set; } 

    public Director Director { get; set; } = null!;

    public MovieViewModel(MoviePickerV2Context context, Movie movie)
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


        AllGenres = context.Genres.ToList()!;
    }


    static public void DeleteMovieRelations(Movie movie, MoviePickerV2Context context)
    {
        var mas = context.MoviesActors
            .Where(ma => ma.MovieId == movie.Id).ToList();

        foreach (var ma in mas)
        {
            if (ma != null)
            {
                context.MoviesActors.Remove(ma);
            }
        }

        var mgs = context.MoviesGenres
            .Where(mg => mg.MovieId == movie.Id).ToList();

        foreach (var mg in mgs)
        {
            if (mg != null)
            {
                context.MoviesGenres.Remove(mg);
            }
        }


        var mls = context.MoviesLanguages
            .Where(ml => ml.MovieId == movie.Id).ToList();

        foreach (var ml in mls)
        {
            if (ml != null)
            {
                context.MoviesLanguages.Remove(ml);
            }
        }
        context.SaveChanges();
    }

    public static void DeleteMovie(Movie movie, MoviePickerV2Context context)
    {
        MovieViewModel.DeleteMovieRelations(movie, context);
        context.Movies.Remove(movie);
        context.SaveChanges();
    }

    

}


