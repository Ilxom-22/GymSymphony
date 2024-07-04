using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class SubscriptionRepository(AppDbContext appDbContext)
    : EntityRepositoryBase<AppDbContext, Subscription>(appDbContext),
        ISubscriptionRepository
{
    public async ValueTask<Subscription?> GetByStripeSubscriptionId(string stripeSubscriptionId, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(s => s.StripeSubscriptionId == stripeSubscriptionId, queryOptions)
            .Include(s => s.LastSubscriptionPeriod)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<Subscription> UpdateAsync(Subscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(subscription, saveChanges, cancellationToken);
    }
}