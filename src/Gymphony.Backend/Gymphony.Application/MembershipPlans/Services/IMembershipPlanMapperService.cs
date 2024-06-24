using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.MembershipPlans.Services;

public interface IMembershipPlanMapperService
{
    MembershipPlansStatusGroupDto MapToGroupedPlans(IEnumerable<MembershipPlan> plans);

    PublicMembershipPlansStatusDto MapToPublicGroupedPlans(IEnumerable<MembershipPlan> plans);
}