using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeSubscriptionItemBroker(
    StripeSubscriptionService stripeSubscriptionService,
    StripeSubscriptionItemService stripeSubscriptionItemService)
    : IStripeSubscriptionItemBroker
{
    public async ValueTask<bool> UpdateSubscriptionToNewPrice(string subscriptionId, string newPriceId,
        CancellationToken cancellationToken = default)
    {
        var subscription = await stripeSubscriptionService.GetAsync(subscriptionId, cancellationToken: cancellationToken);

        var subscriptionItemId = subscription.Items.Data[0].Id;

        var options = new SubscriptionItemUpdateOptions
        {
            Price = newPriceId,
            ProrationBehavior = "none"
        };

        await stripeSubscriptionItemService.UpdateAsync(subscriptionItemId, options, cancellationToken: cancellationToken);

        return true;
    }
}