using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class CourseImage : Entity
{
    public Guid CourseId { get; set; }

    public Guid StorageFileId { get; set; }

    public Course? Course { get; set; }

    public StorageFile? StorageFile { get; set; }
}
