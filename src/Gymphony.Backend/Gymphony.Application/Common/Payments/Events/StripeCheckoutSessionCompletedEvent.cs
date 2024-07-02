using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Payments.Events;

public class StripeCheckoutSessionCompletedEvent : EventBase
{
    public StripeSubscriptionDto Subscription { get; set; } = default!;
}