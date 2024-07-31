using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Queries;

namespace Gymphony.Application.Courses.Queries;

public class GetCourseSchedulesQuery : IQuery<IEnumerable<CourseScheduleDto>>
{
    public Guid CourseId { get; set; }

    public bool IsActiveOnly { get; set; }
}
