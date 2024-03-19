using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class DirectorViewModel
{
    private MoviePickerV2Context _context;
    public Director Director { get; set; } = null!;

    public List<Movie> MoviesDirected { get; set; }


    public DirectorViewModel(MoviePickerV2Context context, Director director)
    {
        _context = context;
        Director = director;

        MoviesDirected = context.Movies
            .Where(m => m.DirectorId == director.Id)
            .ToList()!;
    }


    public void DeleteDirector()
    {
        var moviesByThisDirector = _context.Movies
            .Where(m => m.DirectorId == Director.Id).ToList();

        foreach (var movie in moviesByThisDirector)
        {
            if (movie != null)
            {
                MovieViewModel.DeleteMovie(movie, _context);
                //_context.Movies.Remove(movie);
            }
        }

        _context.Directors.Remove(Director);
        _context.SaveChanges();
    }
}
