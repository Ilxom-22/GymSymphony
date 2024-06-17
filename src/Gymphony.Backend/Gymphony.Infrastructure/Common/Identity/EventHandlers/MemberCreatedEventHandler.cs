using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Infrastructure.Common.Identity.EventHandlers;

public class MemberCreatedEventHandler(IEventBusBroker eventBusBroker) 
    : IEventHandler<MemberCreatedEvent>
{
    public async Task Handle(MemberCreatedEvent notification, CancellationToken cancellationToken)
    {
        await eventBusBroker.PublishLocalAsync(new SystemWelcomeNotificationRequestedEvent
        {
            Recipient = notification.Member
        });
    }
}