using Gymphony.Domain.Structs;

namespace Gymphony.Application.MembershipPlans.Models.Dtos;

public class DraftMembershipPlanDto
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
    
    public Duration Duration { get; set; }
    
    public decimal Price { get; set; }
}