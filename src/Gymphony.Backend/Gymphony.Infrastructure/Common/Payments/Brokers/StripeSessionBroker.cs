using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe.Checkout;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeSessionBroker(StripeSessionService stripeSessionService) : IStripeSessionBroker
{
    public async ValueTask<Session> CreateAsync(SessionCreateOptions sessionCreateOptions, CancellationToken cancellationToken = default)
    {
        return await stripeSessionService
            .CreateAsync(sessionCreateOptions, cancellationToken: cancellationToken);
    }
}