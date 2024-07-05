namespace Gymphony.Domain.Entities;

public class MembershipPlan : Product
{
    public virtual ICollection<MembershipPlanSubscription>? Subscriptions { get; set; }
}