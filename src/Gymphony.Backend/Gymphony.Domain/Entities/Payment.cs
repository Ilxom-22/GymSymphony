using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class Payment : Entity
{
    public Guid MemberId { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset Date { get; set; }

    public Member? Member { get; set; }

    public SubscriptionPeriod? SubscriptionPeriod { get; set; }
}