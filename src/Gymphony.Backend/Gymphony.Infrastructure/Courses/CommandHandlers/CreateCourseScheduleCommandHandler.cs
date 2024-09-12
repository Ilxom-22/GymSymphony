using AutoMapper;
using FluentValidation;
using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Services;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Entities;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Courses.CommandHandlers;

public class CreateCourseScheduleCommandHandler(IMapper mapper,
    IValidator<CreateCourseScheduleCommand> validator,
    ITimeService timeService,
    ICourseRepository courseRepository,
    IStaffRepository staffRepository) 
    : ICommandHandler<CreateCourseScheduleCommand, CourseScheduleDto>
{
    public async Task<CourseScheduleDto> Handle(CreateCourseScheduleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        var newSchedule = mapper.Map<CourseSchedule>(request);

        var foundCourse = await courseRepository.Get(course => course.Id == request.CourseId)
            .Include(course => course.Schedules)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        if (ScheduleTimeOverlaps(foundCourse.Schedules, newSchedule))
            throw new ArgumentException($"The new schedule time on {newSchedule.Day} overlaps with an existing course schedule.");

        var instructors = await staffRepository.GetByIdsAsync(request.InstructorsIds, cancellationToken);

        if (InstructorsScheduleTimeOverlaps(instructors, newSchedule))
            throw new ArgumentException($"The new schedule time on {newSchedule.Day} overlaps with an instructor's existing schedule!");

        newSchedule.Instructors = instructors;

        if (foundCourse.Schedules is null)
            foundCourse.Schedules = [newSchedule];
        else 
            foundCourse.Schedules.Add(newSchedule);

        await courseRepository.UpdateAsync(foundCourse, cancellationToken: cancellationToken);

        return mapper.Map<CourseScheduleDto>(newSchedule);
    }

    public bool ScheduleTimeOverlaps(ICollection<CourseSchedule>? existingCourseSchedules, CourseSchedule newSchedule)
    {
        if (existingCourseSchedules is null || existingCourseSchedules.Count == 0)
            return false;

        foreach (var schedule in existingCourseSchedules)
        {
            if (schedule.Day == newSchedule.Day
                && timeService.IsTimeOverlapping(schedule.StartTime, schedule.EndTime, newSchedule.StartTime, newSchedule.EndTime))
                return true;
        }

        return false;
    }

    public bool InstructorsScheduleTimeOverlaps(ICollection<Staff> instructors, CourseSchedule newSchedule)
    {
        foreach (var schedule in instructors.Select(i => i.CourseSchedules))
        {
            if (ScheduleTimeOverlaps(schedule, newSchedule))
                return true;
        }

        return false;
    }
   
}
