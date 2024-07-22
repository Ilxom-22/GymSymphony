using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.StorageFiles.Models.Settings;

public class AzureBlobStorageSettings
{
    public StorageFileType StorageFileType { get; set; }

    public List<string> AllowedImageExtensions { get; set; } = default!;

    public int MinimumImageSizeInBytes { get; set; }

    public int MaximumImageSizeInBytes { get; set; }

    public string ContainerName { get; set; } = default!;

    public string VirtualPath { get; set; } = default!;
}
