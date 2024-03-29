using MoviePickerDomain.Model;

namespace MoviePickerInfrastructure.Services;

public interface IImportService<TEntity> where TEntity : Entity
{
    Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
}
