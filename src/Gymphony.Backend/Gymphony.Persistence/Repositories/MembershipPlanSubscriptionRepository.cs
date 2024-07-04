using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class MembershipPlanSubscriptionRepository(AppDbContext appDbContext)
    : EntityRepositoryBase<AppDbContext, MembershipPlanSubscription>(appDbContext), 
        IMembershipPlanSubscriptionRepository
{
    public async ValueTask<MembershipPlanSubscription?> GetLatestSubscriptionByMemberId(Guid memberId, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base
            .Get(s => s.MemberId == memberId, queryOptions)
            .Include(s => s.LastSubscriptionPeriod)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<MembershipPlanSubscription> CreateAsync(MembershipPlanSubscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(subscription, saveChanges, cancellationToken);
    }

    public new ValueTask<MembershipPlanSubscription> UpdateAsync(MembershipPlanSubscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(subscription, saveChanges, cancellationToken);
    }
}