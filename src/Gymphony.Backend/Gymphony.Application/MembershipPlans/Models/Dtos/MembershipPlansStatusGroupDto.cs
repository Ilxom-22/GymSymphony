namespace Gymphony.Application.MembershipPlans.Models.Dtos;

public class MembershipPlansStatusGroupDto
{
    public IEnumerable<MembershipPlanDto> DraftPlans { get; set; } = default!;

    public IEnumerable<MembershipPlanDto> PublishedPlans { get; set; } = default!;

    public IEnumerable<MembershipPlanDto> ActivatedPlans { get; set; } = default!;

    public IEnumerable<MembershipPlanDto> DeactivationRequestedPlans { get; set; } = default!;
    
    public IEnumerable<MembershipPlanDto> DeactivatedPlans { get; set; } = default!;
}