using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Domain.Common.Commands;

namespace Gymphony.Application.Subscriptions.Commands;

public class SubscribeForMembershipPlanCommand : ICommand<CheckoutSessionDto>
{
    public Guid MembershipPlanId { get; set; }

    public string SuccessUrl { get; set; } = default!;

    public string CancelUrl { get; set; } = default!;
}