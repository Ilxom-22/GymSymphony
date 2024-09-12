using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Queries;

namespace Gymphony.Application.Courses.Queries;

public class GetPublicCourseByIdQuery : IQuery<CourseDto>
{
    public Guid CourseId { get; set; }
}
