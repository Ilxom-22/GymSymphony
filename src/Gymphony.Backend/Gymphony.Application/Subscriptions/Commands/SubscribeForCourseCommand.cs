using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Subscriptions.Commands;

public class SubscribeForCourseCommand : ICommand<CheckoutSessionDto>
{
    public Guid CourseId { get; set; }

    public ICollection<Guid> SchedulesIds { get; set; } = default!;

    public string SuccessUrl { get; set; } = default!;

    public string CancelUrl { get; set; } = default!;
}
