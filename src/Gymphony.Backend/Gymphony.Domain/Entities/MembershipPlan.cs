namespace Gymphony.Domain.Entities;

public class MembershipPlan : Product
{
    public ICollection<MembershipPlanSubscription>? Subscriptions { get; set; }
}