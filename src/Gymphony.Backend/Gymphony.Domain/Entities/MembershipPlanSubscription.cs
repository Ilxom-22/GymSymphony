namespace Gymphony.Domain.Entities;

public class MembershipPlanSubscription : Subscription
{
    public Guid MembershipPlanId { get; set; }

    public MembershipPlan? MembershipPlan { get; set; }
}