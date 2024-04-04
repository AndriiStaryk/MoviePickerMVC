using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public class MovieDataPortServiceFactory : IDataPortServiceFactory<Movie>
{
    private readonly MoviePickerV2Context _context;
    public MovieDataPortServiceFactory(MoviePickerV2Context context)
    {
        _context = context;
    }
    public IImportService<Movie> GetImportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new MovieImportService(_context);
        }
        throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
    }
    public IExportService<Movie> GetExportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new MovieExportService(_context);
        }
        throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
    }
}
