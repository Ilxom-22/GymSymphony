using AutoMapper;
using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class PublishCourseCommandHandler(ICourseRepository courseRepository, IMapper mapper, IMediator mediator) : ICommandHandler<PublishCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(PublishCourseCommand request, CancellationToken cancellationToken)
    {
        var foundCourse = await courseRepository.Get(course => course.Id == request.CourseId)
            .Include(course => course.Schedules)
            .Include(course => course.StripeDetails)
            .Include(course => course.CourseImages!)
            .ThenInclude(course => course.StorageFile)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        if (foundCourse.Schedules is null || foundCourse.Schedules.Count < foundCourse.EnrollmentsCountPerWeek)
            throw new InvalidEntityStateChangeException<Course>($"Schedules for course with id {request.CourseId} are not complete yet!");

        if (foundCourse.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<Course>($"Publishing Courses is only allowed for courses in Draft status.");

        if (foundCourse.CourseImages is null || foundCourse.CourseImages.Count == 0)
            throw new InvalidEntityStateChangeException<Course>($"Upload at least one image of a course to publish it.");

        if (request.ActivationDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("The course activation time cannot be set for a date and time that has already passed.");

        foundCourse.ActivationDate = request.ActivationDate;
        foundCourse.Status = request.ActivationDate == DateOnly.FromDateTime(DateTime.UtcNow)
            ? ContentStatus.Activated
            : ContentStatus.Published;

        foundCourse.StripeDetails = await GetUpdatedStripeDetails(foundCourse, cancellationToken);

        await courseRepository.UpdateAsync(foundCourse, cancellationToken: cancellationToken);

        return mapper.Map<CourseDto>(foundCourse);
    }

    private async ValueTask<StripeDetails> GetUpdatedStripeDetails(Course course,
    CancellationToken cancellationToken = default)
    {
        var productDetails = mapper.Map<StripeProductDetails>(course);

        if (course.StripeDetails is not null)
            return await mediator.Send(new UpdateStripeDetailsCommand
            {
                StripeDetails = course.StripeDetails,
                UpdatedProductDetails = productDetails
            }, cancellationToken);

        return await mediator.Send(new CreateStripeDetailsCommand
        {
            ProductDetails = productDetails
        }, cancellationToken);
    }
}
