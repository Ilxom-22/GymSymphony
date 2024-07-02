using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripeSubscriptionDto
{
    public string CustomerId { get; set; } = default!;

    public string SubscriptionId { get; set; } = default!;

    public string ProductId { get; set; } = default!;

    public ProductType ProductType { get; set; }

    public long PaymentAmount { get; set; }

    public DateTimeOffset PaymentDate { get; set; }

    public DateTime SubscriptionStartDate { get; set; }

    public DateTime SubscriptionEndDate { get; set; }
}