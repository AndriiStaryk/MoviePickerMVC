using Microsoft.EntityFrameworkCore;
using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class DirectorViewModel
{
    private MoviePickerContext _context;
    public Director Director { get; set; } = null!;

    public List<Movie> MoviesDirected { get; set; }


    public DirectorViewModel(MoviePickerContext context, Director director)
    {
        _context = context;
        Director = director;

        MoviesDirected = context.Movies
            .Where(m => m.DirectorId == director.Id)
            .ToList()!;
    }
}
