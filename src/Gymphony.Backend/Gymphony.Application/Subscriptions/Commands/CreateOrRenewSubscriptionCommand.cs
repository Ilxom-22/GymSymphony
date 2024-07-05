using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Subscriptions.Commands;

public class CreateOrRenewSubscriptionCommand : ICommand<bool>
{
    public Guid MemberId { get; set; }
    
    public Guid ProductId { get; set; }

    public ProductType ProductType { get; set; }

    public string StripeSubscriptionId { get; set; } = default!;

    public DateTime SubscriptionStartDate { get; set; }

    public DateTime SubscriptionEndDate { get; set; }
    
    public decimal PaymentAmount { get; set; }

    public DateTimeOffset PaymentDate { get; set; }
}