using FluentValidation;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Enums;

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

        RuleFor(plan => plan.DurationUnit)
            .NotNull()
            .Must(BeDurationUnitEnum)
            .WithMessage("DurationUnit must me an enum!");

        RuleFor(plan => plan.DurationCount)
            .NotNull()
            .GreaterThan(byte.MinValue);

        RuleFor(plan => plan.Price)
            .NotNull()
            .GreaterThan(decimal.One);
    }

    private bool BeDurationUnitEnum(string durationUnit) =>
         Enum.TryParse(durationUnit, false, out DurationUnit duration);
}