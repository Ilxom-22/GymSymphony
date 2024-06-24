using AutoMapper;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.MembershipPlans.Services;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.MembershipPlans.Services;

public class MembershipPlanMapperService(IMapper mapper) : IMembershipPlanMapperService
{
    public MembershipPlansStatusGroupDto MapToGroupedPlans(IEnumerable<MembershipPlan> plans)
    {
        var groupedPlans = GroupPlansByStatus(plans);
        
        var result = new MembershipPlansStatusGroupDto
        {
            DraftPlans = MapMembershipPlans(groupedPlans, ContentStatus.Draft),
            PublishedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Published),
            ActivatedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Activated),
            DeactivationRequestedPlans = MapMembershipPlans(groupedPlans, ContentStatus.DeactivationRequested),
            DeactivatedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Deactivated),
        };

        return result;
    }

    public PublicMembershipPlansStatusDto MapToPublicGroupedPlans(
        IEnumerable<MembershipPlan> plans)
    {
        var groupedPlans = GroupPlansByStatus(plans);

        var result = new PublicMembershipPlansStatusDto
        {
            ActivatedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Activated),
            PublishedPlans = MapMembershipPlans(groupedPlans, ContentStatus.Published)
        };

        return result;
    }
    
    private Dictionary<ContentStatus, List<MembershipPlan>> GroupPlansByStatus(IEnumerable<MembershipPlan> plans)
    {
        return plans
            .GroupBy(p => p.Status)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
    
    IEnumerable<MembershipPlanDto> MapMembershipPlans(Dictionary<ContentStatus, List<MembershipPlan>> groupedPlans, ContentStatus status)
    {
        return groupedPlans.TryGetValue(status, out var plan)
            ? mapper.Map<IEnumerable<MembershipPlanDto>>(plan)
            : Enumerable.Empty<MembershipPlanDto>();
    }
}