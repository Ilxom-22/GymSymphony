using Stripe.Checkout;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeSessionBroker
{
    ValueTask<Session> CreateAsync(SessionCreateOptions sessionCreateOptions,
        CancellationToken cancellationToken = default);
}