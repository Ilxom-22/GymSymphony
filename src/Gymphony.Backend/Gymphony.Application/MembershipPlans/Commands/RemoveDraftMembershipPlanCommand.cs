using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.MembershipPlans.Commands;

public class RemoveDraftMembershipPlanCommand : ICommand<bool>
{
    public Guid MembershipPlanId { get; set; }
}