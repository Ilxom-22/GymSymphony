using FluentValidation;
using Gymphony.Application.Common.Identity.Models.Dtos;

namespace Gymphony.Infrastructure.Common.Identity.Validators;

public class SignUpDetailsValidator : AbstractValidator<SignUpDetails>
{
    public SignUpDetailsValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(64);

        RuleFor(user => user.LastName)
            .MinimumLength(2)
            .MaximumLength(64);

        RuleFor(user => user.EmailAddress)
            .NotEmpty()
            .WithMessage("Email Address field can't be empty!")
            .EmailAddress()
            .WithMessage("Invalid Email Address! Please try again!");

        RuleFor(user => user.AuthData)
            .NotEmpty()
            .WithMessage("Password field can't be empty!")
            .MinimumLength(8)
            .WithMessage("Password should be at least 8 characters length!")
            .MaximumLength(16)
            .WithMessage("Password length should not exceed 16 characters!");

        RuleFor(user => user.BirthDay)
            .Must(BeAValidAge).WithMessage("User must be at least 18 years old")
            .Must(BeInThePast).WithMessage("Birthday must be a date in the past");
    }
    
    private bool BeAValidAge(DateTimeOffset? birthday)
    {
        if (!birthday.HasValue)
            return true;
        
        var today = DateTime.Today;
        var age = today.Year - birthday.Value.Year;
        if (birthday.Value.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }

    private bool BeInThePast(DateTimeOffset? birthday)
    {
        if (!birthday.HasValue)
            return true;
        
        return birthday < DateTime.Today;
    }

}