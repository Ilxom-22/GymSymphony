using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class AdminRepository(AppDbContext dbContext) : 
    EntityRepositoryBase<AppDbContext, Admin>(dbContext),
    IAdminRepository
{
    public new IQueryable<Admin> Get(
        Expression<Func<Admin, bool>>? predicate = default,
        QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<Admin?> GetByIdAsync(
        Guid adminId, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(admin => admin.Id == adminId, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public int GetActiveAdminsCount(QueryOptions queryOptions = default)
    {
        return base.Get(admin => admin.Status == AccountStatus.Active, queryOptions)
            .Count();
    }

    public new ValueTask<Admin> CreateAsync(
        Admin admin, 
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(admin, saveChanges, cancellationToken);
    }

    public new ValueTask<Admin> UpdateAsync(
        Admin admin,
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(admin, saveChanges, cancellationToken);
    }
}