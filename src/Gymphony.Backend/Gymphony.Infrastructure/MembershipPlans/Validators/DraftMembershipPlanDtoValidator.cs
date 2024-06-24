using FluentValidation;
using Gymphony.Application.MembershipPlans.Models.Dtos;

namespace Gymphony.Infrastructure.MembershipPlans.Validators;

public class DraftMembershipPlanDtoValidator : AbstractValidator<DraftMembershipPlanDto>
{
    public DraftMembershipPlanDtoValidator()
    {
        RuleFor(plan => plan.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(plan => plan.Description)
            .NotEmpty()
            .MaximumLength(2048);

        RuleFor(plan => plan.Duration)
            .NotNull()
            .Custom((duration, context) =>
            {
                if (duration is { Months: 0, Days: 0 })
                    context.AddFailure("Months and days can't be 0.");
                
                else if (duration.Days > 28)
                    context.AddFailure("Days must be less than a month.");
            });

        RuleFor(plan => plan.Price)
            .NotNull()
            .GreaterThan(decimal.One);
    }
}