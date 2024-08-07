using Gymphony.Application.MembershipPlans.Models.Dtos;

namespace Gymphony.Application.Subscriptions.Models.Dtos;

public class MembershipPlanSubscriptionDto
{
    public Guid Id { get; set; }

    public SubscriberMembershipPlanDto MembershipPlan { get; set; } = default!;

    public DateOnly StartDate { get; set; }

    public DateOnly ExpiryDate { get; set; }
}
