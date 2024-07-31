using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class UserProfileImage : Entity
{
    public Guid StorageFileId { get; set; }

    public Guid UserId { get; set; }

    public StorageFile? StorageFile { get; set; }

    public User? User { get; set; }
}
