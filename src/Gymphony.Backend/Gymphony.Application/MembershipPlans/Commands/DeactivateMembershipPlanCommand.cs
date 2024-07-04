using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.MembershipPlans.Commands;

public class DeactivateMembershipPlanCommand : ICommand<MembershipPlanDto>
{
    public Guid MembershipPlanId { get; set; }
}