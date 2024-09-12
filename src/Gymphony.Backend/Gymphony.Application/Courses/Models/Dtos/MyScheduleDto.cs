using Gymphony.Application.Common.Identity.Models.Dtos;

namespace Gymphony.Application.Courses.Models.Dtos;

public class MyScheduleDto
{
    public Guid Id { get; set; }

    public string CourseName { get; set; } = default!;

    public DayOfWeek Day { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public ICollection<StaffDto> Instructors { get; set; } = default!;
}
