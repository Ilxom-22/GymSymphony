using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Notifications.Commands;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Constants;
using Gymphony.Domain.Enums;
using MediatR;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class AdminUnblockedNotificationRequestedEventHandler(
    IMediator mediator, IEventBusBroker eventBusBroker)
    : IEventHandler<AdminUnblockedNotificationRequestedEvent>
{
    public async Task Handle(AdminUnblockedNotificationRequestedEvent notification, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new RetrieveTemplateAsNotificationMessageCommand
            { TemplateType = NotificationType.AdminUnblockedNotification }, cancellationToken);

        message.NotificationMethod = NotificationMethod.Email;
        message.Recipient = notification.Recipient;
        message.Status = NotificationStatus.Pending;
        message.Variables = new()
        {
            { NotificationPlaceholderConstants.FirstName, notification.Recipient.FirstName },
            { NotificationPlaceholderConstants.LastName, notification.Recipient.LastName },
            { NotificationPlaceholderConstants.CompanyName, NotificationPlaceholderConstants.CompanyNameVariable }
        };

        await eventBusBroker.PublishLocalAsync(new NotificationMessageGeneratedEvent { Message = message }, cancellationToken);
    }
}