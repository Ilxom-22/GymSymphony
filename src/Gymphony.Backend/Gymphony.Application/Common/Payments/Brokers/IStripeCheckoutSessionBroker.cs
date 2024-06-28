using Stripe.Checkout;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeCheckoutSessionBroker
{
    ValueTask<Session> CreateAsync(SessionCreateOptions sessionCreateOptions,
        CancellationToken cancellationToken = default);
}