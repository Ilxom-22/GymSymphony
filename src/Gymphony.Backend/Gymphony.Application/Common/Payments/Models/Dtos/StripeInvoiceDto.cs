namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripeInvoiceDto
{
    public string CustomerId { get; set; } = default!;

    public string SubscriptionId { get; set; } = default!;

    public long PaymentAmount { get; set; }

    public DateTimeOffset PaymentDate { get; set; }
}