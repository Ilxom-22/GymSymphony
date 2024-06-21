using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gymphony.Persistence.Interceptors;

public class UpdateAuditableInterceptor(IRequestContextProvider requestContextProvider) 
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var auditableEntries = eventData.Context!.ChangeTracker
            .Entries<IAuditableEntity>().ToList();

        var creationAuditableEntries = eventData.Context!.ChangeTracker.Entries<ICreationAuditableEntity>().ToList();

        var modificationAuditableEntries = eventData.Context!.ChangeTracker
            .Entries<IModificationAuditableEntity>().ToList();

        auditableEntries.ForEach(entry =>
        {
            if (entry.State == EntityState.Modified)
                entry.Property(nameof(IAuditableEntity.ModifiedTime)).CurrentValue = DateTimeOffset.UtcNow;

            if (entry.State == EntityState.Added)
                entry.Property(nameof(IAuditableEntity.CreatedTime)).CurrentValue = DateTimeOffset.UtcNow;
        });
        
        creationAuditableEntries.ForEach(entry =>
        {
            if (entry.State == EntityState.Added)
                entry.Property(nameof(ICreationAuditableEntity.CreatedByUserId)).CurrentValue =
                    requestContextProvider.GetUserIdFromClaims();
        });
        
        modificationAuditableEntries.ForEach(entry =>
        {
            if (entry.State == EntityState.Modified)
                entry.Property(nameof(IModificationAuditableEntity.ModifiedByUserId)).CurrentValue =
                    requestContextProvider.GetUserIdFromClaims();
        });
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}