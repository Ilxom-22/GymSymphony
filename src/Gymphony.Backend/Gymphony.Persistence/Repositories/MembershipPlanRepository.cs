using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class MembershipPlanRepository(AppDbContext appDbContext) :
    EntityRepositoryBase<AppDbContext, MembershipPlan>(appDbContext),
    IMembershipPlanRepository
{
    public new IQueryable<MembershipPlan> Get(Expression<Func<MembershipPlan, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<MembershipPlan?> GetByIdAsync(Guid id, QueryOptions queryOptions = default, CancellationToken cancellationToken = default)
    {
        return await base.Get(plan => plan.Id == id, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async ValueTask<bool> MembershipPlanExistsAsync(string name, Guid membershipPlanId = new Guid(),
        CancellationToken cancellationToken = default)
    {
        return await base
            .Get(queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking))
            .AnyAsync(plan => plan.Name == name && plan.Id != membershipPlanId, cancellationToken);
    }

    public new ValueTask<MembershipPlan> CreateAsync(MembershipPlan membershipPlan, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(membershipPlan, saveChanges, cancellationToken);
    }

    public new ValueTask<MembershipPlan> UpdateAsync(MembershipPlan membershipPlan, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(membershipPlan, saveChanges, cancellationToken);
    }

    public new ValueTask<MembershipPlan> DeleteAsync(MembershipPlan membershipPlan, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(membershipPlan, saveChanges, cancellationToken);
    }
}