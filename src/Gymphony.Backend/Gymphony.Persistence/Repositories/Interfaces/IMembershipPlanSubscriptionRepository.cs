using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IMembershipPlanSubscriptionRepository
{
    ValueTask<MembershipPlanSubscription?> GetLatestSubscriptionByMemberId(Guid memberId,
        QueryOptions queryOptions = default, CancellationToken cancellationToken = default);
    
    ValueTask<MembershipPlanSubscription> CreateAsync(MembershipPlanSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<MembershipPlanSubscription> UpdateAsync(MembershipPlanSubscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default);
}