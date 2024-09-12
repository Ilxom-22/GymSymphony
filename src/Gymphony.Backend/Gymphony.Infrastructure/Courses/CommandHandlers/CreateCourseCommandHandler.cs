using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.StorageFiles.Commands;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class CreateCourseCommandHandler(IMapper mapper, IMediator mediator, 
    IValidator<DraftCourseDto> validator, ICourseRepository courseRepository)
    : ICommandHandler<CreateCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request.Course, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        if (await courseRepository.CourseExistsAsync(request.Course.Name, cancellationToken))
            throw new ArgumentException($"Course with name '{request.Course.Name}' already exists!");

        await courseRepository.BeginTransactionAsync();

        try
        {
            var course = mapper.Map<Course>(request.Course);
            course.Status = ContentStatus.Draft;

            await courseRepository.CreateAsync(course, cancellationToken: cancellationToken);

            var uploadCourseImageCommand = mapper.Map<UploadCourseImageCommand>(request.Course.CourseImage);
            uploadCourseImageCommand.CourseId = course.Id;
            var courseImage = await mediator.Send(uploadCourseImageCommand, cancellationToken);

            var courseDto = mapper.Map<CourseDto>(course);
            courseDto.Image = courseImage;
            await courseRepository.CommitTransactionAsync();

            return courseDto;
        }
        catch
        {
            await courseRepository.RollbackTransactionAsync();
            throw;
        }
    }
}
