using System.Text.RegularExpressions;
using FluentValidation;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Application.Common.Notifications.Models.Settings;
using Gymphony.Domain.Common.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class NotificationMessageGeneratedEventHandler(
    IEventBusBroker eventBusBroker,
    IServiceProvider serviceProvider,
    IOptions<NotificationTemplateRegexPatterns> notificationTemplateRegexPatterns) 
    : IEventHandler<NotificationMessageGeneratedEvent>
{
    private readonly NotificationTemplateRegexPatterns _notificationTemplateRegexPatterns =
        notificationTemplateRegexPatterns.Value;
    
    public async Task Handle(NotificationMessageGeneratedEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var notificationMessageValidator = scope.ServiceProvider
            .GetRequiredService<IValidator<NotificationMessage>>();
        
        var validationResult = await notificationMessageValidator
            .ValidateAsync(notification.Message, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors[0].ToString());

        foreach (var placeholder in notification.Message.Variables)
        {
            notification.Message.Title.Replace(placeholder.Key, placeholder.Value);
            notification.Message.Content.Replace(placeholder.Key, placeholder.Value);
        }

        var missingPlaceholders = Regex.Matches(notification.Message.Title.ToString() + notification.Message.Content, _notificationTemplateRegexPatterns.PlaceholderPattern);
        
        if (missingPlaceholders.Count != 0)
        {
            var missingVariables = missingPlaceholders.Select(variable => variable.ToString());
            
            throw new InvalidOperationException($"Variables for given placeholders are not found - {string.Join(',', missingVariables)}");
        }

        notification.Message.IsRendered = true;

        await eventBusBroker.PublishLocalAsync(new NotificationMessageRenderedEvent { Message = notification.Message }, cancellationToken);
    }   
}