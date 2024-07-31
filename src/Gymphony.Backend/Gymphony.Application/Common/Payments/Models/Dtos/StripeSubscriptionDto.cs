using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Payments.Models.Dtos;

public class StripeSubscriptionDto
{
    public string Id { get; set; } = default!;

    public string ProductId { get; set; } = default!;
    
    public DateTime SubscriptionStartDate { get; set; }

    public DateTime SubscriptionEndDate { get; set; }
}