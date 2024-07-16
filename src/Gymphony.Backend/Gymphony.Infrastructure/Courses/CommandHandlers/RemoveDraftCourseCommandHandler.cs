using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Courses.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class RemoveDraftCourseCommandHandler(ICourseRepository courseRepository) : ICommandHandler<RemoveDraftCourseCommand, bool>
{
    public async Task<bool> Handle(RemoveDraftCourseCommand request, CancellationToken cancellationToken)
    {
        var foundCourse = await courseRepository
            .GetByIdAsync(request.CourseId, cancellationToken: cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        if (foundCourse.Status != ContentStatus.Draft)
            throw new InvalidEntityStateChangeException<MembershipPlan>(
                $"Courses in Draft status can only be deleted!");

        await courseRepository.DeleteAsync(foundCourse, cancellationToken: cancellationToken);

        return true;
    }
}
