namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeSubscriptionItemBroker
{
    ValueTask<bool> UpdateSubscriptionToNewPrice(string subscriptionId, string newPriceId, CancellationToken cancellationToken = default);
}