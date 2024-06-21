using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Infrastructure.Common.Identity.EventHandlers;

public class AdminCreatedEventHandler(IEventBusBroker eventBusBroker)
    : IEventHandler<AdminCreatedEvent>
{
    public async Task Handle(AdminCreatedEvent notification, CancellationToken cancellationToken)
    {
        await eventBusBroker.PublishLocalAsync(new AdminWelcomeNotificationRequestedEvent() 
        {
            Recipient = notification.Admin,
            TemporaryPassword = notification.TemporaryPassword
        }, cancellationToken);
    }
}