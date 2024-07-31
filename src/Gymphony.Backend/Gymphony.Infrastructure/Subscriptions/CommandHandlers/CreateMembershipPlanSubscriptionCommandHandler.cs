using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Subscriptions.CommandHandlers;

public class CreateMembershipPlanSubscriptionCommandHandler(IServiceProvider serviceProvider)
    : ICommandHandler<CreateMembershipPlanSubscriptionCommand, bool>
{
    public async Task<bool> Handle(CreateMembershipPlanSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var membershipPlanSubscriptionRepository =
            scope.ServiceProvider.GetRequiredService<IMembershipPlanSubscriptionRepository>();
            
        var membershipPlanSubscription = new MembershipPlanSubscription
        {
            MemberId = request.MemberId,
            MembershipPlanId = request.MembershipPlanId,
            StripeSubscriptionId = request.StripeSubscriptionId
        };

        await membershipPlanSubscriptionRepository
            .CreateAsync(membershipPlanSubscription, cancellationToken: cancellationToken);

        request.SubscriptionPeriod.SubscriptionId = membershipPlanSubscription.Id;
        membershipPlanSubscription.LastSubscriptionPeriod = request.SubscriptionPeriod;
        
        await membershipPlanSubscriptionRepository.UpdateAsync(membershipPlanSubscription,
            cancellationToken: cancellationToken);

        return true;
    }
}