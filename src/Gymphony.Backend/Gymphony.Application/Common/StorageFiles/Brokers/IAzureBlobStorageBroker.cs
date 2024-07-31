using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.StorageFiles.Brokers;

public interface IAzureBlobStorageBroker
{
    ValueTask<StorageFile> UploadAsync(UploadFileInfoDto file, CancellationToken cancellationToken = default);

    ValueTask<StorageFile> DeleteAsync(StorageFile file, CancellationToken cancellationToken = default);
}
