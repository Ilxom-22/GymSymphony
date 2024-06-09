using Gymphony.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gymphony.Persistence.Interceptors;

public class UpdatePrimaryKeyInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var entities = eventData.Context!.ChangeTracker.Entries<IEntity>().ToList();
        
        entities.ForEach(entry =>
        {
            if (entry.State == EntityState.Added &&
                entry.Properties.Any(property => property.Metadata.Name.Equals(nameof(IEntity.Id))))
                entry.Property(nameof(IEntity.Id)).CurrentValue = Guid.NewGuid();
        });

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}