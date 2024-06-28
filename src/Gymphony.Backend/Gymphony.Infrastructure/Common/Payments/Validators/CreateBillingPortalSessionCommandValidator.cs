using FluentValidation;
using Gymphony.Application.Common.Payments.Commands;

namespace Gymphony.Infrastructure.Common.Payments.Validators;

public class CreateBillingPortalSessionCommandValidator : AbstractValidator<CreateBillingPortalSessionCommand>
{
    public CreateBillingPortalSessionCommandValidator()
    {
        RuleFor(command => command.ReturnUrl)
            .NotEmpty()
            .WithMessage("Return url can't be an empty string.");
    }
}