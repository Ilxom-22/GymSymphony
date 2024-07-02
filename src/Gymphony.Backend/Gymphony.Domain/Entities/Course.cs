namespace Gymphony.Domain.Entities;

public class Course : Product
{
    public int Capacity { get; set; }

    public int SessionDurationInMinutes { get; set; }

    public int EnrollmentsCountPerWeek { get; set; }
}