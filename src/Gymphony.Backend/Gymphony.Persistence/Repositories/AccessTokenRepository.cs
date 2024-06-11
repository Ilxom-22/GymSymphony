using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class AccessTokenRepository(AppDbContext dbContext) : 
    EntityRepositoryBase<AppDbContext, AccessToken>(dbContext),
    IAccessTokenRepository
{
    public async ValueTask<AccessToken?> GetByUserIdAsync(Guid userId, QueryOptions queryOptions = default)
    {
        var foundToken = await base.Get(token => token.UserId == userId,queryOptions)
            .SingleOrDefaultAsync();

        return foundToken;
    }

    public new ValueTask<AccessToken> CreateAsync(
        AccessToken accessToken,
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(accessToken, saveChanges, cancellationToken);
    }

    public new ValueTask<AccessToken> DeleteAsync(
        AccessToken accessToken,
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(accessToken, saveChanges, cancellationToken);
    }

    public async ValueTask<AccessToken> DeleteByUserIdAsync(
        Guid userId, 
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        var foundToken = await GetByUserIdAsync(userId) 
                         ?? throw new InvalidOperationException($"Access Token with user id {userId} does not exist!");

        return await DeleteAsync(foundToken, saveChanges, cancellationToken);
    }
}