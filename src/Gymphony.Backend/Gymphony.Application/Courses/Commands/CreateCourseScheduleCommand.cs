using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Courses.Commands;

public class CreateCourseScheduleCommand : ICommand<CourseScheduleDto>
{
    public Guid CourseId { get; set; }

    public string Day { get; set; } = default!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public ICollection<Guid> InstructorsIds { get; set; } = default!;
}
