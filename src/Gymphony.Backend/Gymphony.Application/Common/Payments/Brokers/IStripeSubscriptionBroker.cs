using Stripe;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeSubscriptionBroker
{
    ValueTask<Subscription> GetByIdAsync(string stripeSubscriptionId, CancellationToken cancellationToken = default);
}