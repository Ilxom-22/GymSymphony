using System.Security.Authentication;
using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Gymphony.Infrastructure.Subscriptions.CommandHandlers;

public class SubscribeForMembershipPlanCommandHandler(
    IMapper mapper, IMediator mediator,
    IRequestContextProvider requestContextProvider,
    IValidator<SubscribeForMembershipPlanCommand> commandValidator,
    IMemberRepository memberRepository,
    IMembershipPlanSubscriptionRepository membershipPlanSubscriptionRepository,
    IMembershipPlanRepository membershipPlanRepository) 
    : ICommandHandler<SubscribeForMembershipPlanCommand, CheckoutSessionDto>
{
    public async Task<CheckoutSessionDto> Handle(SubscribeForMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ToString());
        
        var memberId = requestContextProvider.GetUserIdFromClaims()
                       ?? throw new AuthenticationException("Unauthorized access!");

        var member = await memberRepository
                         .Get(member => member.Id == memberId)
                         .FirstOrDefaultAsync(cancellationToken)
                     ?? throw new AuthenticationException("Unauthorized access!");

        var latestSubscription = await membershipPlanSubscriptionRepository
            .GetLatestSubscriptionByMemberId(member.Id, new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken);
        
        if (latestSubscription is not null && latestSubscription.LastSubscriptionPeriod?.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException($"Member with id {member.Id} cannot subscribe for the Membership Plan with id {request.MembershipPlanId} as he/she might already has the subscription for the given or another Membership Plan");
        
        var membershipPlan = await membershipPlanRepository
                                 .Get(plan => plan.Id == request.MembershipPlanId)
                                 .Include(plan => plan.StripeDetails)
                                 .FirstOrDefaultAsync(cancellationToken)
                             ?? throw new ArgumentException($"Membership Plan with id {request.MembershipPlanId} does not exist!");

        if (membershipPlan.StripeDetails is null || membershipPlan.Status != ContentStatus.Activated)
            throw new ArgumentException($"Product with id {request.MembershipPlanId} is not available for purchasing!");

        var checkoutSessionCommand = mapper.Map<CreateCheckoutSessionCommand>(request);
        checkoutSessionCommand.Member = member;
        checkoutSessionCommand.PriceId = membershipPlan.StripeDetails.PriceId;

        return await mediator.Send(checkoutSessionCommand, cancellationToken);
    }
}