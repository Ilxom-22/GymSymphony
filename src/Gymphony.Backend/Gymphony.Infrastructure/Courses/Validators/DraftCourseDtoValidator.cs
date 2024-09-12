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
            .WithMessage("Duration Period can't be empty!")
            .Must(BeEnum)
            .WithMessage("Duration Unit must me an enum!");

        RuleFor(course => course.DurationCount)
            .NotNull()
            .WithMessage("Number of Periods can't be empty!")
            .GreaterThan(byte.MinValue)
            .WithMessage("Number of Periods should be greater than 0!");

        RuleFor(course => course.Price)
            .NotNull()
            .GreaterThan(decimal.Zero);

        RuleFor(course => course.Capacity)
            .NotEmpty()
            .GreaterThan(byte.MinValue);

        RuleFor(course => course.SessionDurationInMinutes)
            .NotEmpty()
            .WithMessage("Session Duration can't be empty.")
            .GreaterThan(10)
            .WithMessage("Session Duration should be greater than 10.")
            .LessThan(500)
            .WithMessage("Session Duration should be less than 500.");

        RuleFor(course => course.EnrollmentsCountPerWeek)
            .NotEmpty()
            .WithMessage("Minimum sessions count per week should not be empty.")
            .GreaterThan(0)
            .WithMessage("Minimum sessions count per week should be greater than 0.")
            .LessThan(8)
            .WithMessage("Minimum sessions count per week should be less than 8.");
    }

    private bool BeEnum(string durationUnit) =>
        Enum.TryParse(durationUnit, false, out DurationUnit duration);
}
