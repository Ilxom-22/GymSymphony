using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Payments.Events;

public class StripeInvoicePaymentSucceededEvent : EventBase
{
    public StripeInvoiceDto Invoice { get; set; } = default!;
}