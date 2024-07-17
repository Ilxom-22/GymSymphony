using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Payments.Events;

public class StripeCheckoutSessionFailedEvent : EventBase
{
    public string SessionId { get; set; } = default!;
}
