using AutoMapper;
using FluentValidation;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class CreateMembershipPlanCommandHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository,
    IValidator<DraftMembershipPlanDto> membershipPlanValidator)
    : ICommandHandler<CreateMembershipPlanCommand, MembershipPlanDto>
{
    public async Task<MembershipPlanDto> Handle(CreateMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await membershipPlanValidator
            .ValidateAsync(request.MembershipPlan, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        if (await membershipPlanRepository.MembershipPlanExistsAsync(request.MembershipPlan.Name, cancellationToken))
            throw new ArgumentException($"Membership plan with name '{request.MembershipPlan.Name}' already exists!");

        var membershipPlan = mapper.Map<MembershipPlan>(request.MembershipPlan);
        membershipPlan.Status = ContentStatus.Draft;

        await membershipPlanRepository
            .CreateAsync(membershipPlan, cancellationToken: cancellationToken);
        
        return mapper.Map<MembershipPlanDto>(membershipPlan);
    }
}