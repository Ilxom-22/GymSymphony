using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Queries;

namespace Gymphony.Application.Courses.Queries;

public class GetCourseDetailsQuery : IQuery<CourseDetailsDto>
{
    public Guid CourseId { get; set; }
}
