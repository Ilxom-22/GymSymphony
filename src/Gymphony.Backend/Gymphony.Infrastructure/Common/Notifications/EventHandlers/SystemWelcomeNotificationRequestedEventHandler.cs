using System.Text;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Constants;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class SystemWelcomeNotificationRequestedEventHandler(
    IEventBusBroker eventBusBroker,
    IServiceProvider serviceProvider)
    : IEventHandler<SystemWelcomeNotificationRequestedEvent>
{
    public async Task Handle(SystemWelcomeNotificationRequestedEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var notificationTemplateRepository = scope.ServiceProvider.GetRequiredService<INotificationTemplateRepository>();
        
        var notificationTemplate = await notificationTemplateRepository
            .GetByType(NotificationType.SystemWelcome, 
                new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
            ?? throw new EntityNotFoundException<NotificationTemplate>($"Requested notification template not found.");

        var notificationMessage = new NotificationMessage
        {
            TemplateId = notificationTemplate.Id,
            Recipient = notification.Recipient,
            Title = new StringBuilder(notificationTemplate.Title),
            Content = new StringBuilder(notificationTemplate.Content),
            NotificationMethod = NotificationMethod.Email,
            Status = NotificationStatus.Pending,
            Variables = new()
            {
                { NotificationPlaceholderConstants.FirstName, notification.Recipient.FirstName },
                { NotificationPlaceholderConstants.CompanyName, NotificationPlaceholderConstants.CompanyNameVariable }
            }
        };

        await eventBusBroker.PublishLocalAsync(new NotificationMessageGeneratedEvent 
            { Message = notificationMessage });
    }
}