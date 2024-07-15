using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IMembershipPlanSubscriptionRepository
{
    IQueryable<MembershipPlanSubscription> Get(
        Expression<Func<MembershipPlanSubscription, bool>>? predicate = default,
        QueryOptions queryOptions = default);
    
    ValueTask<MembershipPlanSubscription?> GetLatestSubscriptionByMemberId(Guid memberId,
        QueryOptions queryOptions = default, CancellationToken cancellationToken = default);
    
    ValueTask<MembershipPlanSubscription> CreateAsync(MembershipPlanSubscription subscription, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<MembershipPlanSubscription> UpdateAsync(MembershipPlanSubscription subscription, bool saveChanges = true,
        CancellationToken cancellationToken = default);
}