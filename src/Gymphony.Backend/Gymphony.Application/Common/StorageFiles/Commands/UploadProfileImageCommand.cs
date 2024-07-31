using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.StorageFiles.Commands;

public class UploadProfileImageCommand : ICommand<UserProfileImageDto>
{
    public string ContentType { get; set; } = default!;

    public long Size { get; set; }

    public Stream Source { get; set; } = default!;

    public User User { get; set; } = default!;
}
