using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class VerificationTokenRepository(AppDbContext dbContext) 
    : EntityRepositoryBase<AppDbContext, VerificationToken>(dbContext), 
        IVerificationTokenRepository
{
    public async ValueTask<VerificationToken?> GetByTokenAsync(string token, QueryOptions queryOptions = default, CancellationToken cancellationToken = default)
    {
        return await base.Get(verificationToken => verificationToken.Token == token, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async ValueTask<VerificationToken?> GetByUserIdAsync(Guid userId, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(token => token.UserId == userId, queryOptions)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new ValueTask<VerificationToken> CreateAsync(
        VerificationToken verificationToken, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(verificationToken, saveChanges, cancellationToken);
    }

    public new ValueTask<VerificationToken> DeleteAsync(
        VerificationToken verificationToken, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.DeleteAsync(verificationToken, saveChanges, cancellationToken);
    }
}