using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe.Checkout;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeCheckoutSessionBroker(StripeCheckoutSessionService stripeCheckoutSessionService) : IStripeCheckoutSessionBroker
{
    public async ValueTask<Session> CreateAsync(SessionCreateOptions sessionCreateOptions, CancellationToken cancellationToken = default)
    {
        return await stripeCheckoutSessionService
            .CreateAsync(sessionCreateOptions, cancellationToken: cancellationToken);
    }
}