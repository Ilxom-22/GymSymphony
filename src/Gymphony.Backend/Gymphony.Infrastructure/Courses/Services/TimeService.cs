using Gymphony.Application.Courses.Services;
using Gymphony.Domain.Entities;

namespace Gymphony.Infrastructure.Courses.Services;

public class TimeService : ITimeService
{
    public bool IsTimeOverlapping(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
    {
        return start1 < end2 && start2 < end1;
    }

    public bool IsTimeOverlapping(CourseSchedule schedule1, CourseSchedule schedule2)
    {
        return schedule1.StartTime < schedule2.EndTime && schedule2.StartTime < schedule1.EndTime;
    }
}
