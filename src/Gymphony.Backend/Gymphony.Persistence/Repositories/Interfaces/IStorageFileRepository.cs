using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IStorageFileRepository
{
    ValueTask<StorageFile> DeleteAsync(StorageFile storageFile, bool saveChanges = true, CancellationToken cancellationToken = default);
}
