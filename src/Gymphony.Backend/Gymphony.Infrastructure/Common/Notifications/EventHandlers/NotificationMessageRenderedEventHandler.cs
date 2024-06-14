using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Notifications.Brokers;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class NotificationMessageRenderedEventHandler(
    IEventBusBroker eventBusBroker,
    IEmailSenderBroker emailSenderBroker) 
    : IEventHandler<NotificationMessageRenderedEvent>
{
    public async Task Handle(NotificationMessageRenderedEvent notification, CancellationToken cancellationToken)
    {
        var emailDeliveryResult = emailSenderBroker.Send(notification.Message);

        notification.Message.Status = emailDeliveryResult 
            ? NotificationStatus.Sent : NotificationStatus.Failed;

        notification.Message.SentTime = DateTimeOffset.UtcNow;

        await eventBusBroker.PublishLocalAsync(new NotificationMessageSentEvent
        {
            Message = notification.Message
        });
    }
}