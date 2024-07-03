using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IMembershipPlanSubscriptionRepository
{
    ValueTask<MembershipPlanSubscription> CreateAsync(MembershipPlanSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<MembershipPlanSubscription> UpdateAsync(MembershipPlanSubscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default);
}