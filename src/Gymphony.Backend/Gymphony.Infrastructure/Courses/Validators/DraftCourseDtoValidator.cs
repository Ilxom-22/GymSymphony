using FluentValidation;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Courses.Validators;

public class DraftCourseDtoValidator : AbstractValidator<DraftCourseDto>
{
    public DraftCourseDtoValidator()
    {
        RuleFor(course => course.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(course => course.Description)
            .NotEmpty()
            .MaximumLength(2048);

        RuleFor(course => course.DurationUnit)
            .NotNull()
            .Must(BeEnum)
            .WithMessage("DurationUnit must me an enum!");

        RuleFor(course => course.DurationCount)
            .NotNull()
            .GreaterThan(byte.MinValue);

        RuleFor(course => course.Price)
            .NotNull()
            .GreaterThan(decimal.One);

        RuleFor(course => course.Capacity)
            .NotEmpty()
            .GreaterThan(byte.MinValue);

        RuleFor(course => course.SessionDurationInMinutes)
            .NotEmpty()
            .GreaterThan(10)
            .LessThan(500);

        RuleFor(course => course.EnrollmentsCountPerWeek)
            .NotEmpty()
            .GreaterThan(1)
            .LessThan(7);
    }

    private bool BeEnum(string durationUnit) =>
        Enum.TryParse(durationUnit, false, out DurationUnit duration);
}
