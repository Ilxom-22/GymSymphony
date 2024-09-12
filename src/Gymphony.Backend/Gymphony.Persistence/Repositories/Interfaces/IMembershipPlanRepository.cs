using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IMembershipPlanRepository
{
    IQueryable<MembershipPlan> Get(
        Expression<Func<MembershipPlan, bool>>? predicate = default,
        QueryOptions queryOptions = default);

    ValueTask<MembershipPlan?> GetByIdAsync(Guid id, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<bool> MembershipPlanExistsAsync(string name, Guid membershipPlanId = new Guid(),
        CancellationToken cancellationToken = default);

    ValueTask<MembershipPlan> CreateAsync(MembershipPlan membershipPlan,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<MembershipPlan> UpdateAsync(MembershipPlan membershipPlan,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<MembershipPlan> DeleteAsync(MembershipPlan membershipPlan,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}