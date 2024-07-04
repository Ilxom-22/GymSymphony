using FluentValidation;
using Gymphony.Application.Subscriptions.Commands;

namespace Gymphony.Infrastructure.Subscriptions.Validators;

public class SubscribeForMembershipPlanCommandValidator : AbstractValidator<SubscribeForMembershipPlanCommand>
{
    public SubscribeForMembershipPlanCommandValidator()
    {
        RuleFor(command => command.MembershipPlanId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(command => command.CancelUrl)
            .NotEmpty();

        RuleFor(command => command.SuccessUrl)
            .NotEmpty();
    }
}