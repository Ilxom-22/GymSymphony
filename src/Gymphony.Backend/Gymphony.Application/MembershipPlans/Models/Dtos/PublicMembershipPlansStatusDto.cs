namespace Gymphony.Application.MembershipPlans.Models.Dtos;

public class PublicMembershipPlansStatusDto
{
    public IEnumerable<MembershipPlanDto> ActivatedPlans { get; set; } = default!;
    
    public IEnumerable<MembershipPlanDto> PublishedPlans { get; set; } = default!;
}