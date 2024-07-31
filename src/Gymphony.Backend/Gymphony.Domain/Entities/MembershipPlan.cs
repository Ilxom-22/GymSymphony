namespace Gymphony.Domain.Entities;

public class MembershipPlan : Product
{
    public virtual IList<MembershipPlanSubscription>? Subscriptions { get; set; }
}