using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public abstract class Subscription : Entity
{
    public Guid MemberId { get; set; }

    public string StripeSubscriptionId { get; set; } = default!;

    public SubscriptionType Type { get; set; }

    public Member? Member { get; set; }

    public virtual ICollection<SubscriptionPeriod>? SubscriptionPeriods { get; set; }

    public Guid? LastSubscriptionPeriodId { get; set; }

    public SubscriptionPeriod? LastSubscriptionPeriod { get; set; }
}