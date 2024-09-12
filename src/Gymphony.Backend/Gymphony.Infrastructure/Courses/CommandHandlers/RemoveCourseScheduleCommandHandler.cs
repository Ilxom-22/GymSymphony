using Gymphony.Application.Courses.Commands;
using Gymphony.Domain.Common.Commands;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class RemoveCourseScheduleCommandHandler(ICourseScheduleRepository courseScheduleRepository) : ICommandHandler<RemoveCourseScheduleCommand, bool>
{
    public async Task<bool> Handle(RemoveCourseScheduleCommand request, CancellationToken cancellationToken)
    {
        var courseSchedule = await courseScheduleRepository.Get(schedule => schedule.Id == request.ScheduleId)
            .Include(schedule => schedule.Enrollments)
            .Select(schedule => new
        {
            Schedule = schedule,
            Enrollments = schedule.Enrollments.Count
        })
        .FirstOrDefaultAsync(cancellationToken)
        ?? throw new ArgumentException($"Course Schedule with id {request.ScheduleId} does not exist!");

        if (courseSchedule.Enrollments > 0)
            throw new ArgumentException("There are active enrollments to the chosen course schedule!");

        await courseScheduleRepository.DeleteAsync(courseSchedule.Schedule, cancellationToken: cancellationToken);

        return true;
    }
}
