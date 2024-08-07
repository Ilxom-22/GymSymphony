namespace Gymphony.Application.MembershipPlans.Models.Dtos;

public class SubscriberMembershipPlanDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}
