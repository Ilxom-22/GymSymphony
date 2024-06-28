using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe.BillingPortal;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeBillingPortalSessionBroker(
    StripeBillingPortalSessionService stripeBillingPortalSessionService) 
    : IStripeBillingPortalSessionBroker
{
    public async ValueTask<Session> CreateAsync(SessionCreateOptions options, CancellationToken cancellationToken = default)
    {
        return await stripeBillingPortalSessionService
            .CreateAsync(options, cancellationToken: cancellationToken);
    }
}