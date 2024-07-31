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

public class PasswordResetNotificationRequestedEventHandler(
    IEventBusBroker eventBusBroker,
    IMediator mediator,
    IVerificationTokenGeneratorService verificationTokenGeneratorService,
    IOptions<ApiSettings> apiSettings)
    : IEventHandler<PasswordResetNotificationRequestedEvent>
{
    private readonly ApiSettings _apiSettings = apiSettings.Value;

    public async Task Handle(PasswordResetNotificationRequestedEvent notification, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new RetrieveTemplateAsNotificationMessageCommand
            { TemplateType = NotificationType.PasswordResetVerification }, cancellationToken);

        var passwordResetVerificationToken = verificationTokenGeneratorService
            .GenerateVerificationToken(notification.Recipient, VerificationType.PasswordReset);

        await mediator.Send(new CreateVerificationTokenCommand { VerificationToken = passwordResetVerificationToken }, cancellationToken);

        var passwordResetLink = _apiSettings.PasswordResetUrl + $"?Token={HttpUtility.UrlEncode(passwordResetVerificationToken.Token)}";

        message.NotificationMethod = NotificationMethod.Email;
        message.Recipient = notification.Recipient;
        message.Status = NotificationStatus.Pending;
        message.Variables = new()
        {
            { NotificationPlaceholderConstants.FirstName, notification.Recipient.FirstName },
            { NotificationPlaceholderConstants.PasswordResetLink, passwordResetLink },
            { NotificationPlaceholderConstants.CompanyName, NotificationPlaceholderConstants.CompanyNameVariable }
        };

        await eventBusBroker.PublishLocalAsync(new NotificationMessageGeneratedEvent 
            { Message = message }, cancellationToken);
    }
}