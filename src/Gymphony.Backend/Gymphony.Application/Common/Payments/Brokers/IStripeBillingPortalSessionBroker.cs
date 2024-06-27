using Stripe.BillingPortal;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeBillingPortalSessionBroker
{
    ValueTask<Session> CreateAsync(SessionCreateOptions options,
        CancellationToken cancellationToken = default);
}