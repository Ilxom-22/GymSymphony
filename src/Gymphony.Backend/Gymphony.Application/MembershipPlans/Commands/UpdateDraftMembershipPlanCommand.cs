using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Structs;

namespace Gymphony.Application.MembershipPlans.Commands;

public class UpdateDraftMembershipPlanCommand : ICommand<MembershipPlanDto>
{
    public Guid MembershipPlanId { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
    
    public Duration Duration { get; set; }
    
    public decimal Price { get; set; }
}