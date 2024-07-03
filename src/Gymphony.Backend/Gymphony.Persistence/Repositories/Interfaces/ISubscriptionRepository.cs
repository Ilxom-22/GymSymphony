using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    ValueTask<Subscription?> GetByStripeSubscriptionId(string stripeSubscriptionId, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<Subscription> UpdateAsync(Subscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default);
}