using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.MembershipPlans.Commands;

public class UpdateDraftMembershipPlanCommand : ICommand<MembershipPlanDto>
{
    public Guid MembershipPlanId { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string DurationUnit { get; set; } = default!;

    public byte DurationCount { get; set; }
    
    public decimal Price { get; set; }
}