using AutoMapper;
using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Subscriptions.CommandHandlers;

public class CreateOrRenewSubscriptionCommandHandler(IServiceProvider serviceProvider,
    IMapper mapper, IMediator mediator) 
    : ICommandHandler<CreateOrRenewSubscriptionCommand, bool>
{
    public async Task<bool> Handle(CreateOrRenewSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var subscriptionRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();

        var subscription = await subscriptionRepository
            .GetByStripeSubscriptionId(request.StripeSubscriptionId,
                cancellationToken: cancellationToken);

        var subscriptionPeriod = new SubscriptionPeriod
        {
            StartDate = DateOnly.FromDateTime(request.SubscriptionStartDate),
            ExpiryDate = DateOnly.FromDateTime(request.SubscriptionEndDate),
            Payment = new Payment
            {
                Amount = request.PaymentAmount,
                MemberId = request.MemberId,
                Date = request.PaymentDate
            }
        };

        if (subscription is not null)
        {
            subscription.LastSubscriptionPeriod = subscriptionPeriod;
            await subscriptionRepository.UpdateAsync(subscription, cancellationToken: cancellationToken);
            
            return true;
        }

        if (request.ProductType == ProductType.MembershipPlan)
        {
            var command = mapper.Map<CreateMembershipPlanSubscriptionCommand>(request);
            command.SubscriptionPeriod = subscriptionPeriod;

            return await mediator.Send(command, cancellationToken);
        }

        return true;
    }
}