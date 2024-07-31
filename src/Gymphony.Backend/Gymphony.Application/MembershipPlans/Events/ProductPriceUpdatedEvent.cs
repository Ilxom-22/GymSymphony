using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.MembershipPlans.Events;

public class ProductPriceUpdatedEvent : EventBase
{
    public Product Product { get; set; } = default!;
}