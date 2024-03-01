using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models;

public class MovieViewModel
{

    public Movie Movie { get; set; } = null!;

    public List<Genre> Genres { get; set; } = null!;

    public List<Genre> SelectedGenres { get; set; } = null!;
    public MoviesGenre moviesGenre { get; set; } = null!;   
}
