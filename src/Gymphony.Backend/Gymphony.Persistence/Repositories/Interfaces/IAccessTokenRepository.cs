using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IAccessTokenRepository
{
    ValueTask<AccessToken?> GetByUserIdAsync(Guid userId,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);
    
    ValueTask<AccessToken> CreateAsync(
        AccessToken accessToken,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<AccessToken> DeleteAsync(
        AccessToken accessToken,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<AccessToken> DeleteByUserIdAsync(
        Guid userId, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}