using Azure.Storage.Blobs;
using Gymphony.Application.Common.StorageFiles.Brokers;
using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Application.Common.StorageFiles.Models.Settings;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Gymphony.Infrastructure.Common.StorageFiles.Brokers;

public class AzureBlobStorageBroker(BlobServiceClient blobServiceClient, IOptions<StorageFileSettings> storageFileSettings) 
    : IAzureBlobStorageBroker
{
    private readonly List<AzureBlobStorageSettings> _azureBlobStorageSettings = storageFileSettings.Value.Settings.ToList();

    public async ValueTask<StorageFile> UploadAsync(UploadFileInfoDto file, CancellationToken cancellationToken = default)
    {
        var blobClient = CreateNewBlobClient(file);

        await blobClient.UploadAsync(file.Source, cancellationToken);
        
        return new StorageFile { FileName = blobClient.Name, Type = file.StorageFileType, Url = blobClient.Uri.AbsoluteUri };
    }

    public async ValueTask<StorageFile> DeleteAsync(StorageFile file, CancellationToken cancellationToken = default)
    {
        var blobContainerClient = GetBlobContainerClient(file.Type);
        var blobClient = blobContainerClient.GetBlobClient(file.FileName);

        await blobClient.DeleteAsync(cancellationToken: cancellationToken);

        return file;
    }

    private BlobClient CreateNewBlobClient(UploadFileInfoDto file)
    {
        var fileSettings = _azureBlobStorageSettings
            .SingleOrDefault(s => s.StorageFileType == file.StorageFileType)
            ?? throw new ArgumentException($"Unsupported file type: {file.StorageFileType}");

        var blobContainerClient = blobServiceClient.GetBlobContainerClient(fileSettings.ContainerName);

        var blobName = $"{fileSettings.VirtualPath}/{Guid.NewGuid()}.{file.ContentType.Split('/')[1]}";

        return blobContainerClient.GetBlobClient(blobName);
    } 

    private BlobContainerClient GetBlobContainerClient(StorageFileType storageFileType)
    {
        var fileSettings = _azureBlobStorageSettings
            .SingleOrDefault(s => s.StorageFileType == storageFileType)
            ?? throw new ArgumentException($"Unsupported file type: {storageFileType}");

        var blobContainerClient = blobServiceClient.GetBlobContainerClient(fileSettings.ContainerName);

        return blobContainerClient;
    }
}
