using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Courses.Commands;

public class PublishCourseCommand : ICommand<CourseDto>
{
    public Guid CourseId { get; set; }

    public DateOnly ActivationDate { get; set; }
}
