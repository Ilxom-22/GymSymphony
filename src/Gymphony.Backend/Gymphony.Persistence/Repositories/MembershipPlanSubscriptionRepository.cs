using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class MembershipPlanSubscriptionRepository(AppDbContext appDbContext)
    : EntityRepositoryBase<AppDbContext, MembershipPlanSubscription>(appDbContext), 
        IMembershipPlanSubscriptionRepository
{
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