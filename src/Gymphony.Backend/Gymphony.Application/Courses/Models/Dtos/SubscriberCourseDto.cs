using Gymphony.Application.Common.StorageFiles.Models.Dtos;

namespace Gymphony.Application.Courses.Models.Dtos;

public class SubscriberCourseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int SessionDurationInMinutes { get; set; }

    public CourseImageDto Image { get; set; } = default!;
}
