using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.MembershipPlans.Events;

public class MembershipPlanPriceUpdatedEvent : EventBase
{
    public MembershipPlan MembershipPlan { get; set; } = default!;
}