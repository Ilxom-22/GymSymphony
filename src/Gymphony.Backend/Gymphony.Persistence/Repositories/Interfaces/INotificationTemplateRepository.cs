using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Persistence.Repositories.Interfaces;

public interface INotificationTemplateRepository
{
    ValueTask<NotificationTemplate?> GetByType(
        NotificationType type, 
        QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default);
}