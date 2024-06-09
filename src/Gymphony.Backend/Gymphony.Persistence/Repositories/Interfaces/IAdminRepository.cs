using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IAdminRepository
{
    IQueryable<Admin> Get(
        Expression<Func<Admin, bool>>? predicate = default,
        QueryOptions queryOptions = default);
    
    ValueTask<Admin> CreateAsync(
        Admin admin, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}