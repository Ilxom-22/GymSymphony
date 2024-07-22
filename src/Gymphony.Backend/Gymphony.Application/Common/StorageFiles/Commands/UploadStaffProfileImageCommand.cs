using Gymphony.Application.Common.StorageFiles.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Common.StorageFiles.Commands;

public class UploadStaffProfileImageCommand : ICommand<UserProfileImageDto>
{
    public string ContentType { get; set; } = default!;

    public long Size { get; set; }

    public Stream Source { get; set; } = default!;

    public Guid StaffId { get; set; }
}
