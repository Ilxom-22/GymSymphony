using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public class StorageFile : Entity
{
    public string FileName { get; set; } = default!;

    public StorageFileType Type { get; set; }

    public string Url { get; set; } = default!;
}
