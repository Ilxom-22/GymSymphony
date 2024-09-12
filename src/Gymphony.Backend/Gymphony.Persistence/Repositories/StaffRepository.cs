using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gymphony.Persistence.Repositories;

public class StaffRepository(AppDbContext dbContext) : EntityRepositoryBase<AppDbContext, Staff>(dbContext), IStaffRepository
{
    public new IQueryable<Staff> Get(Expression<Func<Staff, bool>>? predicate = default, QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<IList<Staff>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await base.Get()
                    .Where(staff => ids.Contains(staff.Id))
                    .Include(staff => staff.ProfileImage!)
                    .ThenInclude(image => image.StorageFile)
                    .ToListAsync(cancellationToken);
    }

    public new ValueTask<Staff> CreateAsync(Staff staff, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(staff, saveChanges, cancellationToken);
    }

    public new ValueTask<Staff> DeleteAsync(Staff staff, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(staff, saveChanges, cancellationToken);
    }
}
