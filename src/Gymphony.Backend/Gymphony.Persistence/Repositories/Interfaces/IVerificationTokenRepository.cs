using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface IVerificationTokenRepository
{
    ValueTask<VerificationToken?> GetByTokenAsync(string token,
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<VerificationToken?> GetByUserIdAsync(Guid userId, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);

    ValueTask<VerificationToken> CreateAsync(
        VerificationToken verificationToken, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    ValueTask<VerificationToken> DeleteAsync(
        VerificationToken verificationToken,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}