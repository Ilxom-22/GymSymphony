namespace Gymphony.Domain.Entities;

public class Staff : User
{
    public string Bio { get; set; } = default!;

    public bool TemporaryPasswordChanged { get; set; } = false;
    
    public virtual ICollection<Course>? Courses { get; set; }

    public virtual ICollection<CourseSchedule>? CourseSchedules { get; set; }
}