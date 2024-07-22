namespace Gymphony.Domain.Entities;

public class Course : Product
{
    public int Capacity { get; set; }

    public int SessionDurationInMinutes { get; set; }

    public int EnrollmentsCountPerWeek { get; set; }

    public ICollection<CourseImage>? CourseImages { get; set; }

    public virtual ICollection<Staff>? Instructors { get; set; }

    public virtual ICollection<CourseSchedule>? Schedules { get; set; }

    public virtual ICollection<CourseSubscription>? Subscriptions { get; set; }
}