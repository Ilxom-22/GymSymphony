using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class UpdateDraftMembershipPlanCommandHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository,
    IValidator<DraftMembershipPlanDto> membershipPlanValidator)
    : ICommandHandler<UpdateDraftMembershipPlanCommand, MembershipPlanDto>
{
    public async Task<MembershipPlanDto> Handle(UpdateDraftMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var draftMembershipPlanDto = mapper.Map<DraftMembershipPlanDto>(request);
        
        var validationResult = await membershipPlanValidator
            .ValidateAsync(draftMembershipPlanDto, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());
        
        var foundPlan = await membershipPlanRepository
            .GetByIdAsync(request.MembershipPlanId, cancellationToken: cancellationToken)
            ?? throw new ArgumentException($"Membership plan with id '{request.MembershipPlanId}' does not exist!");

        if (foundPlan.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<MembershipPlan>($"Updating Membership plans is only allowed for plans in Draft status. To modify this plan, please deactivate it first.");

        foundPlan.Name = request.Name;
        foundPlan.Description = request.Description;
        foundPlan.Duration = request.Duration;
        foundPlan.Price = request.Price;

        await membershipPlanRepository
            .UpdateAsync(foundPlan, cancellationToken: cancellationToken);
        
        return mapper.Map<MembershipPlanDto>(foundPlan);
    }
}