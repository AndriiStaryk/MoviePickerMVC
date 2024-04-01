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



    public static async Task<bool> IsDirectorExist(string name,
                                                    DateOnly birthDate,
                                                    int birthCountryID,
                                                    IFormFile? directorImage,
                                                    MoviePickerV2Context context)
    {
        byte[]? image = [];
        if (directorImage != null && directorImage.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await directorImage.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }
        else
        {
            string imagePath = "C:\\Users\\Andrii\\source\\repos\\MoviePickerWebApplication_v2\\src\\MoviePickerMVC\\MoviePickerInfrastructure\\wwwroot\\Images\\no_person_image.jpg";
            if (System.IO.File.Exists(imagePath))
            {
                byte[] defaultImageBytes = System.IO.File.ReadAllBytes(imagePath);
                image = defaultImageBytes;
            }
        }

        var director = await context.Directors.FirstOrDefaultAsync(d => d.Name == name &&
                                                                     d.BirthDate == birthDate &&
                                                                     d.BirthCountryId == birthCountryID);

        if (director != null && image != null && director.DirectorImage!.SequenceEqual(image))
        {
            return true;
        }

        return false;
    }


}
