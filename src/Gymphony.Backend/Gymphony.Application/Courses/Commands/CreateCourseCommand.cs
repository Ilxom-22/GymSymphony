using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Courses.Commands;

public class CreateCourseCommand : ICommand<CourseDto>
{
    public DraftCourseDto Course { get; set; } = default!;
}
