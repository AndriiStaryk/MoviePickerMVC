using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public class DirectorDataPortServiceFactory : IDataPortServiceFactory<Director>
{
    private readonly MoviePickerV2Context _context;
    public DirectorDataPortServiceFactory(MoviePickerV2Context context)
    {
        _context = context;
    }
    public IImportService<Director> GetImportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new DirectorImportService(_context);
        }
        throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
    }
    public IExportService<Director> GetExportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new DirectorExportService(_context);
        }
        throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
    }

}