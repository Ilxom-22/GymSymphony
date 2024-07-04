using Gymphony.Domain.Common.Entities;

namespace Gymphony.Domain.Entities;

public class SubscriptionPeriod : Entity
{
    public Guid SubscriptionId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public Guid PaymentId { get; set; }

    public Payment? Payment { get; set; }

    public Subscription? Subscription { get; set; }
}