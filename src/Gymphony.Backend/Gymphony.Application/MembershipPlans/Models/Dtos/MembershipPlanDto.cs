using Gymphony.Domain.Structs;

namespace Gymphony.Application.MembershipPlans.Models.Dtos;

public class MembershipPlanDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public string Description { get; set; } = default!;

    public Duration Duration { get; set; }

    public string Status { get; set; } = default!;

    public DateOnly? ActivationDate { get; set; }

    public decimal Price { get; set; }
}