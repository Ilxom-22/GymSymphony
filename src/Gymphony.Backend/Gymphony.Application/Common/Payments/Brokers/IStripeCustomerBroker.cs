using Stripe;

namespace Gymphony.Application.Common.Payments.Brokers;

public interface IStripeCustomerBroker
{
    ValueTask<Customer> CreateAsync(CustomerCreateOptions options, 
        CancellationToken cancellationToken = default);
}