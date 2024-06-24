using AutoMapper;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Events;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class UpdateMembershipPlanPriceCommandHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository,
    IEventBusBroker eventBusBroker) 
    : ICommandHandler<UpdateMembershipPlanPriceCommand, MembershipPlanDto>
{
    public async Task<MembershipPlanDto> Handle(UpdateMembershipPlanPriceCommand request, CancellationToken cancellationToken)
    {
        var foundPlan = await membershipPlanRepository
            .GetByIdAsync(request.MembershipPlanId, cancellationToken: cancellationToken)
            ?? throw new ArgumentException($"Membership plan with id '{request.MembershipPlanId}' does not exist!");

        if (foundPlan.Status is ContentStatus.DeactivationRequested or ContentStatus.Deactivated)
            throw new ArgumentException(
                "Updating the price is not allowed for membership plans that are deactivated or have a deactivation request in progress!");

        if (request.Price < 1)
            throw new ArgumentException("Price must be greater than 1");

        foundPlan.Price = request.Price;
        await membershipPlanRepository.UpdateAsync(foundPlan, cancellationToken: cancellationToken);

        if (foundPlan.Status is ContentStatus.Activated)
            await eventBusBroker.PublishLocalAsync(new MembershipPlanPriceUpdatedEvent
                { MembershipPlan = foundPlan }, cancellationToken);

        return mapper.Map<MembershipPlanDto>(foundPlan);
    }
}