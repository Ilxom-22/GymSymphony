using Gymphony.Application.Common.Identity.Models.Dtos;

namespace Gymphony.Application.Courses.Models.Dtos;

public class CourseScheduleDto
{
    public Guid Id { get; set; }

    public Guid CourseId { get; set; }

    public string Day { get; set; } = default!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public ICollection<StaffDto> Instructors { get; set; } = default!;

    public bool IsAvaliable { get; set; }
}
