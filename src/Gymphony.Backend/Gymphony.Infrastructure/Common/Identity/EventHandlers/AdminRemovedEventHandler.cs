using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Infrastructure.Common.Identity.EventHandlers;

public class AdminRemovedEventHandler(IEventBusBroker eventBusBroker) : IEventHandler<AdminRemovedEvent>
{
    public async Task Handle(AdminRemovedEvent notification, CancellationToken cancellationToken)
    {
        await eventBusBroker.PublishLocalAsync(new AdminRemovedNotificationRequestedEvent() 
        {
            Recipient = notification.RemovedAdmin
        });
    }
}