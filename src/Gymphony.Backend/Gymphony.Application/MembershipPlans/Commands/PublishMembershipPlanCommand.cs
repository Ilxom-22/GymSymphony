using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.MembershipPlans.Commands;

public class PublishMembershipPlanCommand : ICommand<MembershipPlanDto>
{
    public Guid MembershipPlanId { get; set; }

    public DateOnly ActivationDate { get; set; }
}