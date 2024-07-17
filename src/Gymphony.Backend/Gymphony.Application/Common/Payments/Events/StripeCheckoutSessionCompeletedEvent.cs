using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Payments.Events;

public class StripeCheckoutSessionCompeletedEvent : EventBase
{
    public string SessionId { get; set; } = default!;

    public string CustomerId { get; set; } = default!;

    public string SubscriptionId { get; set; } = default!;

    public long PaymentAmount { get; set; }

    public DateTimeOffset PaymentDate { get; set; }
}
