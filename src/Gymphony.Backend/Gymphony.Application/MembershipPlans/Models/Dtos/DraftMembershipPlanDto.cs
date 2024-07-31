namespace Gymphony.Application.MembershipPlans.Models.Dtos;

public class DraftMembershipPlanDto
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string DurationUnit { get; set; } = default!;

    public byte DurationCount { get; set; }
    
    public decimal Price { get; set; }
}