using AutoMapper;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class PublishMembershipPlanCommandHandler(IMapper mapper, IMediator mediator,
    IMembershipPlanRepository membershipPlanRepository)
    : ICommandHandler<PublishMembershipPlanCommand, MembershipPlanDto>
{
    public async Task<MembershipPlanDto> Handle(PublishMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var foundPlan = await membershipPlanRepository
            .Get(plan => plan.Id == request.MembershipPlanId)
            .Include(plan => plan.StripeDetails)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Membership plan with id '{request.MembershipPlanId}' does not exist!");
        
        if (foundPlan.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<MembershipPlan>($"Publishing Membership plans is only allowed for plans in Draft status.");

        if (request.ActivationDate < DateOnly.FromDateTime(DateTimeOffset.UtcNow.Date))
            throw new ArgumentException("The membership plan activation time cannot be set for a date and time that has already passed.");

        foundPlan.ActivationDate = request.ActivationDate;
        foundPlan.Status = request.ActivationDate == DateOnly.FromDateTime(DateTimeOffset.UtcNow.Date) 
            ? ContentStatus.Activated 
            : ContentStatus.Published;

        foundPlan.StripeDetails = await GetUpdatedStripeDetails(foundPlan, cancellationToken);

        await membershipPlanRepository
            .UpdateAsync(foundPlan, cancellationToken: cancellationToken);

        return mapper.Map<MembershipPlanDto>(foundPlan);
    }

    private async ValueTask<StripeDetails> GetUpdatedStripeDetails(MembershipPlan plan, 
        CancellationToken cancellationToken = default)
    {
        var productDetails = mapper.Map<StripeProductDetails>(plan);
        
        if (plan.StripeDetails is not null)
            return await mediator.Send(new UpdateStripeDetailsCommand
            {
                StripeDetails = plan.StripeDetails,
                UpdatedProductDetails = productDetails
            }, cancellationToken);
        
        return await mediator.Send(new CreateStripeDetailsCommand
        {
            ProductDetails = productDetails
        }, cancellationToken);
    }
}