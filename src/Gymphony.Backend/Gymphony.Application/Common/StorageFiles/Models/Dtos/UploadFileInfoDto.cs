using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.StorageFiles.Models.Dtos;

public class UploadFileInfoDto
{
    public string ContentType { get; set; } = default!;

    public StorageFileType StorageFileType { get; set; }

    public Stream Source { get; set; } = default!;
}
