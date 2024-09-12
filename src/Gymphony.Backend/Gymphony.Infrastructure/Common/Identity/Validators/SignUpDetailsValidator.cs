using FluentValidation;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Common.Identity.Validators;

public class SignUpDetailsValidator : AbstractValidator<SignUpDetails>
{
    public SignUpDetailsValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("First Name field can't be empty!")
            .MinimumLength(2).WithMessage("First Name should contain at least 2 characters!")
            .MaximumLength(64).WithMessage("First Name should not exceed 64 characters!");

        RuleFor(user => user.LastName)
            .MinimumLength(2).WithMessage("Last Name should contain at least 2 characters!")
            .MaximumLength(64).WithMessage("Last Name should not exceed 64 characters!");

        RuleFor(user => user.EmailAddress)
            .NotEmpty().WithMessage("Email Address field can't be empty!")
            .EmailAddress().WithMessage("Invalid Email Address! Please try again!");

        RuleSet(RuleSets.AdminSignUp.ToString(), () => 
            RuleFor(user => user.AuthData).Empty());

        RuleSet(RuleSets.EmailSignUp.ToString(), () =>
            RuleFor(user => user.AuthData)
                .SetValidator(new PasswordValidator()));
        
        RuleSet(RuleSets.ThirdPartySignUp.ToString(), () =>
            RuleFor(user => user.AuthData)
                .NotEmpty().WithMessage("Password field can't be empty!"));

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