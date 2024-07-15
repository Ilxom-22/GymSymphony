namespace Gymphony.Application.Courses.Models.Dtos;

public class DraftCourseDto
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string DurationUnit { get; set; } = default!;

    public byte DurationCount { get; set; } = default!;

    public int Capacity { get; set; }

    public int SessionDurationInMinutes { get; set; }

    public int EnrollmentsCountPerWeek { get; set; }

    public decimal Price { get; set; }
}
