using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Application.Common.Notifications.Commands;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Application.Common.Settings;
using Gymphony.Application.Common.VerificationTokens.Commands;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Constants;
using Gymphony.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using System.Web;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class EmailVerificationNotificationRequestedEventHandler(
    IMediator mediator,
    IEventBusBroker eventBusBroker,
    IVerificationTokenGeneratorService verificationTokenGeneratorService,
    IOptions<ApiClientSettings> apiClientSettings)
    : IEventHandler<EmailVerificationNotificationRequestedEvent>
{
    private readonly ApiClientSettings _apiClientSettings = apiClientSettings.Value;
    
    public async Task Handle(EmailVerificationNotificationRequestedEvent notification, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new RetrieveTemplateAsNotificationMessageCommand
            { TemplateType = NotificationType.EmailVerification }, cancellationToken);

        var emailVerificationToken = verificationTokenGeneratorService
            .GenerateVerificationToken(notification.Recipient, VerificationType.Email);

        await mediator.Send(new CreateVerificationTokenCommand { VerificationToken = emailVerificationToken }, cancellationToken);

        var verificationLink = _apiClientSettings.EmailVerificationUrl + $"?Token={HttpUtility.UrlEncode(emailVerificationToken.Token)}";
        
        message.NotificationMethod = NotificationMethod.Email;
        message.Recipient = notification.Recipient;
        message.Status = NotificationStatus.Pending;
        message.Variables = new()
        {
            { NotificationPlaceholderConstants.FirstName, notification.Recipient.FirstName },
            { NotificationPlaceholderConstants.VerificationLink, verificationLink },
            { NotificationPlaceholderConstants.CompanyName, NotificationPlaceholderConstants.CompanyNameVariable }
        };

        await eventBusBroker.PublishLocalAsync(new NotificationMessageGeneratedEvent 
            { Message = message }, cancellationToken);
    }
}