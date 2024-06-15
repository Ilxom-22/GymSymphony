using AutoMapper;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class NotificationMessageSentEventHandler(
    IMapper mapper,
    IServiceProvider serviceProvider)
    : IEventHandler<NotificationMessageSentEvent>
{
    public async Task Handle(NotificationMessageSentEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var notificationHistoryRepository = scope.ServiceProvider.GetRequiredService<INotificationHistoryRepository>();
        
        var notificationHistory = mapper.Map<NotificationHistory>(notification.Message);

        await notificationHistoryRepository.CreateAsync(notificationHistory, cancellationToken: cancellationToken);
    }
}