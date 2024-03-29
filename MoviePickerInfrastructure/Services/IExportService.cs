using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public interface IExportService<TEntity> where TEntity : Entity
{
    Task WriteToAsync(Stream stream, CancellationToken cancellationToken);

}
