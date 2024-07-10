using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories;

public class StaffRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, Staff>(dbContext), IStaffRepository
{
    public new IQueryable<Staff> Get(Expression<Func<Staff, bool>>? predicate, QueryOptions queryOptions)
    {
        return base.Get(predicate, queryOptions);
    }

    public new ValueTask<Staff> CreateAsync(Staff staff, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(staff, saveChanges, cancellationToken);
    }

    public new ValueTask<Staff> DeleteAsync(Staff staff, bool saveChanges, CancellationToken cancellationToken)
    {
        return base.DeleteAsync(staff, saveChanges, cancellationToken);
    }
}
