using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Infrastructure.Common.Identity.EventHandlers;

public class AdminBlockedEventHandler(IEventBusBroker eventBusBroker)
    : IEventHandler<AdminBlockedEvent>
{
    public async Task Handle(AdminBlockedEvent notification, CancellationToken cancellationToken)
    {
        await eventBusBroker.PublishLocalAsync(new AdminBlockedNotificationRequestedEvent 
        {
            Recipient = notification.BlockedAdmin
        });
    }
}