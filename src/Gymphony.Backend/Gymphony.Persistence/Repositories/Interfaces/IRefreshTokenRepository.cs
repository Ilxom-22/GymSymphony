using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    ValueTask<RefreshToken?> GetByValueAsync(
        string token,
        QueryOptions queryOptions = default);

    ValueTask<RefreshToken?> GetByUserIdAsync(
        Guid userId,
        QueryOptions queryOptions = default);

    ValueTask<RefreshToken> CreateAsync(
        RefreshToken token, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<RefreshToken> DeleteAsync(
        RefreshToken token,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<RefreshToken> DeleteByUserIdAsync(
        Guid userId,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}