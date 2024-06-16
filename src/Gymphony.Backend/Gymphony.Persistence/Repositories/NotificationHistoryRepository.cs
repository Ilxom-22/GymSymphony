using Gymphony.Domain.Entities;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Persistence.Repositories;

public class NotificationHistoryRepository(AppDbContext appDbContext) :
    EntityRepositoryBase<AppDbContext, NotificationHistory>(appDbContext),
    INotificationHistoryRepository
{
    public new ValueTask<NotificationHistory> CreateAsync(NotificationHistory history, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        return base.CreateAsync(history, saveChanges, cancellationToken);
    }
}