using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class RefreshTokenRepository(AppDbContext dbContext) :
    EntityRepositoryBase<AppDbContext, RefreshToken>(dbContext),
    IRefreshTokenRepository
{
    public async ValueTask<RefreshToken?> GetByValueAsync(
        string token, QueryOptions queryOptions = default)
    {
        var foundToken = await base.Get(refreshToken 
            => refreshToken.Token == token, queryOptions)
            .SingleOrDefaultAsync();

        return foundToken;
    }

    public async ValueTask<RefreshToken?> GetByUserIdAsync(
        Guid userId, QueryOptions queryOptions = default)
    {
        var foundToken = await base.Get(token 
            => token.UserId == userId, queryOptions)
            .SingleOrDefaultAsync();

        return foundToken;
    }

    public new ValueTask<RefreshToken> CreateAsync(
        RefreshToken token,
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(token, saveChanges, cancellationToken);
    }

    public new ValueTask<RefreshToken> DeleteAsync(
        RefreshToken token,
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(token, saveChanges, cancellationToken);
    }

    public async ValueTask<RefreshToken> DeleteByUserIdAsync(
        Guid userId, 
        bool saveChanges = true, 
        CancellationToken cancellationToken = default)
    {
        var foundToken = await GetByUserIdAsync(userId) 
                         ?? throw new InvalidOperationException($"Refresh Token with user id {userId} does not exist!");

        return await DeleteAsync(foundToken, saveChanges, cancellationToken);
    }
}