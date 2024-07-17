using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Subscriptions.Commands;

public class CreateCourseSubscriptionCommand : ICommand<CourseSubscription>
{
    public Guid MemberId { get; set; }
    
    public Guid CourseId { get; set; }

    public string StripeSubscriptionId { get; set; } = default!;

    public SubscriptionPeriod SubscriptionPeriod { get; set; } = default!;
}