using Gymphony.Domain.Entities;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface INotificationHistoryRepository
{
    ValueTask<NotificationHistory> CreateAsync(
        NotificationHistory history, 
        bool saveChanges = true,
        CancellationToken cancellationToken = default);
}