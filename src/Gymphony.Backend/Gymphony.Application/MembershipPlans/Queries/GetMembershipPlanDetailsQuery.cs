using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.MembershipPlans.Queries;

public class GetMembershipPlanDetailsQuery : ICommand<MembershipPlanDetailsDto>
{
    public Guid MembershipPlanId { get; set; }
}