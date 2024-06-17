using FluentValidation;
using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Common.Notifications.Validators;

public class NotificationMessageValidator : AbstractValidator<NotificationMessage>
{
    public NotificationMessageValidator()
    {
        RuleFor(message => message.TemplateId)
            .NotEmpty();

        RuleFor(message => message.Recipient)
            .NotNull();

        RuleFor(message => message.Content)
            .NotNull()
            .NotEmpty();

        RuleFor(message => message.Title)
            .NotNull()
            .NotEmpty();

        RuleFor(message => message.IsRendered)
            .Equal(false);

        RuleFor(message => message.Status)
            .Equal(NotificationStatus.Pending);

        RuleFor(message => message.Variables)
            .NotNull();
    }
}