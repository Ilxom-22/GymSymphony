using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gymphony.Persistence.Interceptors;

public class UpdateSoftDeletionInterceptor(IRequestContextProvider requestContextProvider)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var combinedEntries = eventData.Context!.ChangeTracker
            .Entries<ISoftDeletedEntity>()
            .Where(entry => entry.Entity is IDeletionAuditableEntity)
            .ToList();

        var softDeletedEntries = eventData.Context!.ChangeTracker
            .Entries<ISoftDeletedEntity>()
            .Where(entry => entry.Entity is not IDeletionAuditableEntity)
            .ToList();

        var deletionAuditableEntries = eventData.Context!.ChangeTracker
            .Entries<IDeletionAuditableEntity>()
            .Where(entry => entry.Entity is not ISoftDeletedEntity)
            .ToList();

        combinedEntries.ForEach(entry =>
        {
            if (entry.State != EntityState.Deleted) return;

            entry.State = EntityState.Modified;

            entry.Property(nameof(IDeletionAuditableEntity.DeletedByUserId)).CurrentValue =
                requestContextProvider.GetUserIdFromClaims();

            entry.Property(nameof(ISoftDeletedEntity.IsDeleted)).CurrentValue = true;
            entry.Property(nameof(ISoftDeletedEntity.DeletedTime)).CurrentValue = DateTimeOffset.UtcNow;

            var ownedTypes = entry.References.Where(entity => entity.Metadata.TargetEntityType.IsOwned()).ToList();
            ownedTypes.ForEach(entity => entity.TargetEntry!.State = EntityState.Modified);
        });

        deletionAuditableEntries.ForEach(entry =>
        {
            if (entry.State != EntityState.Deleted) return;

            entry.State = EntityState.Modified;

            entry.Property(nameof(IDeletionAuditableEntity.DeletedByUserId)).CurrentValue =
                requestContextProvider.GetUserIdFromClaims();
        });

        softDeletedEntries.ForEach(entry =>
        {
            if (entry.State != EntityState.Deleted) return;

            entry.State = EntityState.Modified;

            var ownedTypes = entry.References.Where(entity => entity.Metadata.TargetEntityType.IsOwned()).ToList();
            ownedTypes.ForEach(entity => entity.TargetEntry!.State = EntityState.Modified);

            entry.Property(nameof(ISoftDeletedEntity.IsDeleted)).CurrentValue = true;
            entry.Property(nameof(ISoftDeletedEntity.DeletedTime)).CurrentValue = DateTimeOffset.UtcNow;
        });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
