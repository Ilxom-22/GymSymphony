using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.DataContexts;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Persistence.Repositories;

public class NotificationTemplateRepository(AppDbContext dbContext) :
    EntityRepositoryBase<AppDbContext, NotificationTemplate>(dbContext),
    INotificationTemplateRepository
{
    public async ValueTask<NotificationTemplate?> GetByType(NotificationType type, QueryOptions queryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return await base.Get(notification => notification.Type == type, queryOptions).FirstOrDefaultAsync(cancellationToken);
    }
}