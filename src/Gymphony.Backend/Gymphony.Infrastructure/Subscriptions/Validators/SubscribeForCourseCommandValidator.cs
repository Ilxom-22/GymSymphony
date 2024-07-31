using FluentValidation;
using Gymphony.Application.Subscriptions.Commands;

namespace Gymphony.Infrastructure.Subscriptions.Validators;

public class SubscribeForCourseCommandValidator : AbstractValidator<SubscribeForCourseCommand>
{
    public SubscribeForCourseCommandValidator()
    {
        RuleFor(command => command.CourseId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(command => command.SchedulesIds)
            .NotEmpty();

        RuleFor(command => command.CancelUrl)
            .NotEmpty();

        RuleFor(command => command.SuccessUrl)
            .NotEmpty();
    }
}
