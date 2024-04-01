using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public class ActorDataPortServiceFactory : IDataPortServiceFactory<Actor>
{
    private readonly MoviePickerV2Context _context;
    public ActorDataPortServiceFactory(MoviePickerV2Context context)
    {
        _context = context;
    }
    public IImportService<Actor> GetImportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new ActorImportService(_context);
        }
        throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
    }
    public IExportService<Actor> GetExportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            return new ActorExportService(_context);
        }
        throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
    }

}
