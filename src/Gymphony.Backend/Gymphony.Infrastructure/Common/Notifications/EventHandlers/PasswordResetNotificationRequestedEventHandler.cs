using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Application.Common.Notifications.Commands;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Application.Common.VerificationTokens.Commands;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Constants;
using Gymphony.Domain.Enums;
using MediatR;

namespace Gymphony.Infrastructure.Common.Notifications.EventHandlers;

public class PasswordResetNotificationRequestedEventHandler(
    IEventBusBroker eventBusBroker,
    IMediator mediator,
    IVerificationTokenGeneratorService verificationTokenGeneratorService)
    : IEventHandler<PasswordResetNotificationRequestedEvent>
{
    public async Task Handle(PasswordResetNotificationRequestedEvent notification, CancellationToken cancellationToken)
    {
        var message = await mediator.Send(new RetrieveTemplateAsNotificationMessageCommand
            { TemplateType = NotificationType.PasswordResetVerification }, cancellationToken);

        var passwordResetVerificationToken = verificationTokenGeneratorService
            .GenerateVerificationToken(notification.Recipient, VerificationType.PasswordReset);

        await mediator.Send(new CreateVerificationTokenCommand { VerificationToken = passwordResetVerificationToken }, cancellationToken);
        
        message.NotificationMethod = NotificationMethod.Email;
        message.Recipient = notification.Recipient;
        message.Status = NotificationStatus.Pending;
        message.Variables = new()
        {
            { NotificationPlaceholderConstants.FirstName, notification.Recipient.FirstName },
            { NotificationPlaceholderConstants.PasswordResetLink, passwordResetVerificationToken.Token },
            { NotificationPlaceholderConstants.CompanyName, NotificationPlaceholderConstants.CompanyNameVariable }
        };

        await eventBusBroker.PublishLocalAsync(new NotificationMessageGeneratedEvent 
            { Message = message }, cancellationToken);
    }
}