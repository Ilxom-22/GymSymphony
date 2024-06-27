using Gymphony.Application.Common.Payments.Brokers;
using Gymphony.Infrastructure.Common.Payments.Services;
using Stripe;

namespace Gymphony.Infrastructure.Common.Payments.Brokers;

public class StripeCustomerBroker(StripeCustomerService stripeCustomerService) : IStripeCustomerBroker
{
    public async ValueTask<Customer> CreateAsync(CustomerCreateOptions options, CancellationToken cancellationToken = default)
    {
        return await stripeCustomerService.CreateAsync(options, cancellationToken: cancellationToken);
    }
}