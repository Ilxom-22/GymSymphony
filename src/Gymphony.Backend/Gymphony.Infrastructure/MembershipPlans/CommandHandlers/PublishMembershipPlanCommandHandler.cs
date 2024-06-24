using AutoMapper;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class PublishMembershipPlanCommandHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository)
    : ICommandHandler<PublishMembershipPlanCommand, MembershipPlanDto>
{
    public async Task<MembershipPlanDto> Handle(PublishMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var foundPlan = await membershipPlanRepository
            .GetByIdAsync(request.MembershipPlanId, cancellationToken: cancellationToken)
            ?? throw new ArgumentException($"Membership plan with id '{request.MembershipPlanId}' does not exist!");
        
        if (foundPlan.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<MembershipPlan>($"Publishing Membership plans is only allowed for plans in Draft status.");

        if (request.ActivationDate < DateOnly.FromDateTime(DateTimeOffset.UtcNow.Date))
            throw new ArgumentException("The membership plan activation time cannot be set for a date and time that has already passed.");

        foundPlan.ActivationDate = request.ActivationDate;
        foundPlan.Status = request.ActivationDate == DateOnly.FromDateTime(DateTimeOffset.UtcNow.Date) 
            ? ContentStatus.Activated 
            : ContentStatus.Published;

        await membershipPlanRepository
            .UpdateAsync(foundPlan, cancellationToken: cancellationToken);

        return mapper.Map<MembershipPlanDto>(foundPlan);
    }
}