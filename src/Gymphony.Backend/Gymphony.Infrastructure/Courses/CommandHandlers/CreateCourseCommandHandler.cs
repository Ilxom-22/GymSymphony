using AutoMapper;
using FluentValidation;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class CreateCourseCommandHandler(IMapper mapper, IValidator<DraftCourseDto> validator, ICourseRepository courseRepository)
    : ICommandHandler<CreateCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Course, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        if (await courseRepository.CourseExistsAsync(request.Course.Name, cancellationToken))
            throw new ArgumentException($"Course with name '{request.Course.Name}' already exists!");

        var course = mapper.Map<Course>(request.Course);
        course.Status = ContentStatus.Draft;

        await courseRepository.CreateAsync(course, cancellationToken: cancellationToken);

        return mapper.Map<CourseDto>(course);
    }
}
