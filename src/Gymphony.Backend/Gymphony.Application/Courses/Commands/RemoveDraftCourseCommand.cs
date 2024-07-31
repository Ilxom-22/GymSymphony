using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Courses.Commands;

public class RemoveDraftCourseCommand : ICommand<bool>
{
    public Guid CourseId { get; set; }
}
