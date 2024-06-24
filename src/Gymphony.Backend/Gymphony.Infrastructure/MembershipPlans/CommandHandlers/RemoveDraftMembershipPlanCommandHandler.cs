using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class RemoveDraftMembershipPlanCommandHandler(
    IMembershipPlanRepository membershipPlanRepository)
    : ICommandHandler<RemoveDraftMembershipPlanCommand, bool>
{
    public async Task<bool> Handle(RemoveDraftMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var foundPlan = await membershipPlanRepository
                            .GetByIdAsync(request.MembershipPlanId, cancellationToken: cancellationToken)
                        ?? throw new ArgumentException($"Membership plan with id '{request.MembershipPlanId}' does not exist!");

        if (foundPlan.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<MembershipPlan>(
                $"Membership plans in Draft status can only be deleted!");

        await membershipPlanRepository.DeleteAsync(foundPlan, cancellationToken: cancellationToken);

        return true;
    }
}