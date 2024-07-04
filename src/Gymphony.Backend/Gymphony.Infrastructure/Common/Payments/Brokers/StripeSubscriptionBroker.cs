using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeSubscriptionBroker(StripeSubscriptionService stripeSubscriptionService) : IStripeSubscriptionBroker
{
    public async ValueTask<Subscription> GetByIdAsync(string stripeSubscriptionId, CancellationToken cancellationToken = default)
    {
        return await stripeSubscriptionService.GetAsync(stripeSubscriptionId, cancellationToken: cancellationToken);
    }
}