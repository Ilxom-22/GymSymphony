namespace Gymphony.Application.Common.StorageFiles.Models.Dtos;

public class UserProfileImageDto
{
    public Guid ProfileImageId { get; set; }

    public string Url { get; set; } = default!;
}
