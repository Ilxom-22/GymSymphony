using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Infrastructure.Common.Identity.EventHandlers;

public class AdminUnblockedEventHandler(IEventBusBroker eventBusBroker) : IEventHandler<AdminUnblockedEvent>
{
    public async Task Handle(AdminUnblockedEvent notification, CancellationToken cancellationToken)
    {
        await eventBusBroker.PublishLocalAsync(new AdminUnblockedNotificationRequestedEvent 
        {
            Recipient = notification.UnblockedAdmin
        }, cancellationToken);
    }
}