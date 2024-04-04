using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public class MovieExportService : IExportService<Movie>
{
    private readonly MoviePickerV2Context _context;

    public MovieExportService(MoviePickerV2Context context)
    {
        _context = context;
    }

    public Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
