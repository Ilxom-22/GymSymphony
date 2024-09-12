using AutoMapper;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class DeactivateCourseComandHandler(ICourseRepository courseRepository, IMapper mapper) : ICommandHandler<DeactivateCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(DeactivateCourseCommand request, CancellationToken cancellationToken)
    {
        var foundCourse = await courseRepository.Get(course => course.Id == request.CourseId)
                .Include(course => course.CourseImages!)
                .ThenInclude(image => image.StorageFile)
                .Include(course => course.Subscriptions!)
                .ThenInclude(subscription => subscription.LastSubscriptionPeriod)
                .FirstOrDefaultAsync(cancellationToken)
             ?? throw new ArgumentException($"Course with Id {request.CourseId} does not exist!");

        if (foundCourse.Subscriptions is null || foundCourse.Subscriptions.Count == 0)
        {
            foundCourse.Status = ContentStatus.Draft;
            foundCourse.ActivationDate = null;
        }
        else
        {
            var subscriptionWithMaxExpiryDate = foundCourse.Subscriptions
                .Where(s => s.LastSubscriptionPeriod?.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
                .DefaultIfEmpty()
            .Max();

            if (subscriptionWithMaxExpiryDate is null)
            {
                foundCourse.Status = ContentStatus.Deactivated;
                foundCourse.DeactivationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }
            else
            {
                foundCourse.Status = ContentStatus.DeactivationRequested;
                foundCourse.DeactivationDate = subscriptionWithMaxExpiryDate.LastSubscriptionPeriod!.ExpiryDate;
            }
        }

        await courseRepository.UpdateAsync(foundCourse, cancellationToken: cancellationToken);

        return mapper.Map<CourseDto>(foundCourse);
    }
}
