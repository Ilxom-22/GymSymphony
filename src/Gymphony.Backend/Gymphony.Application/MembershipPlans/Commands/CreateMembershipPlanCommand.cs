using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.MembershipPlans.Commands;

public class CreateMembershipPlanCommand : ICommand<MembershipPlanDto>
{
    public DraftMembershipPlanDto MembershipPlan { get; set; } = default!;
}