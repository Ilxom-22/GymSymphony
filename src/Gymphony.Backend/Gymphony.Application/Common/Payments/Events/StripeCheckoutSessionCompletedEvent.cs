using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Payments.Events;

public class StripeCheckoutSessionCompletedEvent : EventBase
{
    public Stripe.Checkout.Session Session { get; set; } = default!;
}