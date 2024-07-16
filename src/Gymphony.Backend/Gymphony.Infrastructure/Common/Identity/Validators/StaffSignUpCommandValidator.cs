using FluentValidation;
using Gymphony.Application.Common.Identity.Commands;

namespace Gymphony.Infrastructure.Common.Identity.Validators;

public class StaffSignUpCommandValidator : AbstractValidator<StaffSignUpCommand>
{
    public StaffSignUpCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotEmpty().WithMessage("First Name field can't be empty!")
            .MinimumLength(2).WithMessage("First Name should contain at least 2 characters!")
            .MaximumLength(64).WithMessage("First Name should not exceed 64 characters!");

        RuleFor(command => command.LastName)
            .MinimumLength(2).WithMessage("Last Name should contain at least 2 characters!")
            .MaximumLength(64).WithMessage("Last Name should not exceed 64 characters!");

        RuleFor(command => command.EmailAddress)
            .NotEmpty().WithMessage("Email Address field can't be empty!")
            .EmailAddress().WithMessage("Invalid Email Address! Please try again!");

        RuleFor(command => command.Bio)
            .NotEmpty().WithMessage("Bio section can't be empty!")
            .MaximumLength(2048);
    }
}
