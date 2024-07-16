using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Courses.Commands;

public class RemoveCourseScheduleCommand : ICommand<bool>
{
    public Guid ScheduleId { get; set; }
}
