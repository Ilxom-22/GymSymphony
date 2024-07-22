using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class StorageFileRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, StorageFile>(dbContext), IStorageFileRepository
{
    public new ValueTask<StorageFile> DeleteAsync(StorageFile storageFile, bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(storageFile, saveChanges, cancellationToken);
    }
}
