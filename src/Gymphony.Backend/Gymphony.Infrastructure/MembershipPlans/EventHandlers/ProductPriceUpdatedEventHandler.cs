using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Application.MembershipPlans.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.MembershipPlans.EventHandlers;

public class ProductPriceUpdatedEventHandler(IServiceProvider serviceProvider,
    IStripeSubscriptionItemBroker stripeSubscriptionItemBroker) 
    : IEventHandler<ProductPriceUpdatedEvent>
{
    public async Task Handle(ProductPriceUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        List<string> stripeSubscriptionIds;

        if (notification.Product.Type == ProductType.MembershipPlan)
        {
            var membershipPlanSubscriptionRepository = scope.ServiceProvider.GetRequiredService<IMembershipPlanSubscriptionRepository>();

            stripeSubscriptionIds = await membershipPlanSubscriptionRepository
                .Get(ms => ms.MembershipPlanId == notification.Product.Id, new QueryOptions(QueryTrackingMode.AsNoTracking))
                .Select(ms => ms.StripeSubscriptionId)
                .ToListAsync(cancellationToken);
        }
        else
        {
            var courseSubscriptionRepository = scope.ServiceProvider.GetRequiredService<ICourseSubscriptionRepository>();

            stripeSubscriptionIds = await courseSubscriptionRepository.Get(cs => cs.CourseId == notification.Product.Id, new QueryOptions(QueryTrackingMode.AsNoTracking))
                .Select(cs => cs.StripeSubscriptionId)
                .ToListAsync(cancellationToken);
        }

        foreach (var subscriptionId in stripeSubscriptionIds)
            await stripeSubscriptionItemBroker.UpdateSubscriptionToNewPrice(subscriptionId, notification.Product.StripeDetails!.PriceId, cancellationToken);
    }
}