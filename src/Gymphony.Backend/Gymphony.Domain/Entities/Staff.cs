namespace Gymphony.Domain.Entities;

public class Staff : User
{
    public virtual ICollection<Course>? Courses { get; set; }

    public virtual ICollection<CourseSchedule>? CourseSchedules { get; set; }
}