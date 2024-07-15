using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Courses.Commands;

public class UpdateDraftCourseCommand : ICommand<CourseDto>
{
    public Guid CourseId { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string DurationUnit { get; set; } = default!;

    public byte DurationCount { get; set; }

    public int Capacity { get; set; }

    public int SessionDurationInMinutes { get; set; }

    public int EnrollmentsCountPerWeek { get; set; }

    public decimal Price { get; set; }
}
