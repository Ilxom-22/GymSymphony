using Gymphony.Application.Common.StorageFiles.Models.Dtos;

namespace Gymphony.Application.Courses.Models.Dtos;

public class CourseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string DurationUnit { get; set; } = default!;

    public int DurationCount { get; set; }

    public int Capacity { get; set; }

    public int SessionDurationInMinutes { get; set; }

    public int EnrollmentsCountPerWeek { get; set; }

    public string Status { get; set; } = default!;

    public DateOnly? ActivationDate { get; set; }

    public DateOnly? DeactivationDate { get; set; }

    public decimal Price { get; set; }

    public CourseImageDto Image { get; set; } = default!;
}
