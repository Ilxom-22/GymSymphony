using FluentValidation;
using Gymphony.Application.Courses.Commands;
using Gymphony.Domain.Enums;

namespace Gymphony.Infrastructure.Courses.Validators;

public class CreateCourseScheduleCommandValidator : AbstractValidator<CreateCourseScheduleCommand>
{
    public CreateCourseScheduleCommandValidator()
    {
        RuleFor(command => command.CourseId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(command => command.InstructorsIds)
            .NotEmpty();

        RuleFor(command => command.Day)
            .Must(BeDayOfWeekEnum);
    }

    private bool BeDayOfWeekEnum(string day) =>
         Enum.TryParse(day, false, out DayOfWeek dayOfWeek);
}
