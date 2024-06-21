using FluentValidation;

namespace Gymphony.Infrastructure.Common.Identity.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotEmpty().WithMessage("Password field can't be empty!")
            .MinimumLength(8).WithMessage("Password should be at least 8 characters in length!")
            .MaximumLength(16).WithMessage("Password length should not exceed 16 characters!")
            .Must(ContainUppercaseLetter).WithMessage("Password must contain at least one capital letter!")
            .Must(ContainLowercaseLetter).WithMessage("Password must contain at least one lowercase letter!")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit!")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character!");
    }

    private bool ContainUppercaseLetter(string password) =>
        password.Any(char.IsUpper);

    private bool ContainLowercaseLetter(string password) => 
        password.Any(char.IsLower);

    private bool ContainDigit(string password) =>
        password.Any(char.IsDigit);
    
    private bool ContainSpecialCharacter(string password) =>
        password.Any(ch => !char.IsLetterOrDigit(ch));
}