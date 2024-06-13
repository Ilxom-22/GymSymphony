using FluentValidation;
using Gymphony.Application.Common.Identity.Models.Dtos;

namespace Gymphony.Infrastructure.Common.Identity.Validators;

public class SignInDetailsValidator : AbstractValidator<SignInDetails>
{
    public SignInDetailsValidator()
    {
        RuleFor(user => user.EmailAddress)
            .NotEmpty()
            .WithMessage("Email Address field can't be empty!")
            .EmailAddress()
            .WithMessage("Invalid Email Address! Please try again!");

        RuleFor(user => user.AuthData)
            .NotEmpty()
            .WithMessage("Password field can't be empty!");
    }
}