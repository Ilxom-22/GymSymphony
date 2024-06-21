using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IUserRepository
{
    ValueTask<bool> UserExists(
        string emailAddress, 
        QueryOptions queryOptions = default);

    IQueryable<User> Get(
        Expression<Func<User, bool>>? predicate,
        QueryOptions queryOptions = default);

    ValueTask<User?> GetByIdAsync(
        Guid id, 
        QueryOptions queryOptions = default, 
        CancellationToken cancellationToken = default);
    
    ValueTask<User?> GetByEmailAddressAsync(
        string emailAddress,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<User> UpdateAsync(
        User user,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}