using DocumentFormat.OpenXml.InkML;
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

        var rs = context.Reviews
            .Where(r => r.MovieId == movie.Id).ToList();

        foreach (var r in rs)
        {
            if (r != null)
            {
                context.Reviews.Remove(r);
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

    public static void CalculateRating(Movie movie, MoviePickerV2Context context)
    {
        var sum = 0.0;

        var reviews = context.Reviews
            .Where(review => review.MovieId == movie.Id).ToList();
        var ratings = reviews.Select(r => r.Rating).ToList();

        if (ratings.Count == 0)
        {
            movie.Rating = 0;
            return;
        }

        foreach(var rating in ratings)
        {
            sum += rating;
        }
        
        movie.Rating = Math.Round(sum / ratings.Count, 1);
    }


    static public async Task<bool> IsMovieExist(string title,
                                     DateOnly releaseDate,
                                     int directorID,
                                     long? budget,
                                     long? boxOfficeRevenue,
                                     int? duration,
                                     double? rating,
                                     string? description,
                                     IFormFile? movieImage,
                                     MoviePickerV2Context context)
    {

        byte[]? image = [];
        if (movieImage != null && movieImage.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await movieImage.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }


        var movie = await context.Movies
            .FirstOrDefaultAsync(
                m => m.Title == title &&
                m.ReleaseDate == releaseDate &&
                m.DirectorId == directorID &&
                m.Budget == budget &&
                m.BoxOfficeRevenue == boxOfficeRevenue &&
                m.Duration == duration &&
                m.Rating == rating &&
                m.Description == description);

        if (movie != null && image != null && movie.MovieImage!.SequenceEqual(image))
        {
            return true;
        }

        return false;
    }


}


