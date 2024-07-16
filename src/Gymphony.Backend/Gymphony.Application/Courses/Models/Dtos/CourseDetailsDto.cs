using Gymphony.Application.Common.Identity.Models.Dtos;

namespace Gymphony.Application.Courses.Models.Dtos;

public class CourseDetailsDto
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

    public DateTimeOffset CreatedTime { get; set; }

    public DateTimeOffset? ModifiedTime { get; set; }

    public UserDto? CreatedBy { get; set; }

    public UserDto? ModifiedBy { get; set; }
}
