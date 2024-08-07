using Gymphony.Application.Courses.Models.Dtos;

namespace Gymphony.Application.Subscriptions.Models.Dtos;

public class CourseSubscriptionDto
{
    public Guid Id { get; set; }

    public SubscriberCourseDto Course { get; set; } = default!;

    public DateOnly StartDate { get; set; } = default!;

    public DateOnly ExpiryDate { get; set; } = default!;
}
