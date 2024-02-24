using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<int> SelectedGenres { get; set; }
    }
}
