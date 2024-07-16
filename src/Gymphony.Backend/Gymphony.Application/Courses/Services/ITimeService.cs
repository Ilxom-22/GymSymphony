using Gymphony.Domain.Entities;

namespace Gymphony.Application.Courses.Services;

public interface ITimeService
{
    bool IsTimeOverlapping(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2);

    bool IsTimeOverlapping(CourseSchedule schedule1, CourseSchedule schedule2);
}
