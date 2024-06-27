using Gymphony.Domain.Common.Events;
using Stripe;

namespace Gymphony.Application.Common.Payments.Events;

public class StripeInvoicePaymentSucceededEvent : EventBase
{
    public Invoice Invoice { get; set; } = default!;
}