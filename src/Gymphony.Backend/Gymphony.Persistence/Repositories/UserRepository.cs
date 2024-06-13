using System.Linq.Expressions;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext) :
    EntityRepositoryBase<AppDbContext, User>(dbContext),
    IUserRepository
{
    public async ValueTask<bool> UserExists(string emailAddress, QueryOptions queryOptions = default)
    {
        return await base.Get(user => user.EmailAddress == emailAddress, queryOptions)
            .AnyAsync();
    }

    public new IQueryable<User> Get(
        Expression<Func<User, bool>>? predicate,
        QueryOptions queryOptions = default)
    {
        return base.Get(predicate, queryOptions);
    }

    public async ValueTask<User?> GetByIdAsync(
        Guid id,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(user => user.Id == id, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async ValueTask<User?> GetByEmailAddressAsync(
        string emailAddress,
        QueryOptions queryOptions = default)
    {
        return await base.Get(user => user.EmailAddress == emailAddress, queryOptions).FirstOrDefaultAsync();
    }

    public new ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return base.UpdateAsync(user, saveChanges, cancellationToken);
    }
}