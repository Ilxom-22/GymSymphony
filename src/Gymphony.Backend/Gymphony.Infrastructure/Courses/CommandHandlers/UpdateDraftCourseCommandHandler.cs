using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class UpdateDraftCourseCommandHandler(IMapper mapper, 
    IValidator<DraftCourseDto> courseValidator, 
    ICourseRepository courseRepository) 
    : ICommandHandler<UpdateDraftCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(UpdateDraftCourseCommand request, CancellationToken cancellationToken)
    {
        var draftCourse = mapper.Map<DraftCourseDto>(request);

        var validationResult = await courseValidator.ValidateAsync(draftCourse, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        var foundCourse = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken: cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        if (foundCourse.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<MembershipPlan>($"Updating Courses is only allowed for courses in Draft status. To modify this course, please deactivate it first.");

        mapper.Map(draftCourse, foundCourse);

        await courseRepository.UpdateAsync(foundCourse, cancellationToken: cancellationToken);

        return mapper.Map<CourseDto>(foundCourse);
    }
}
