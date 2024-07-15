using AutoMapper;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Courses.QueryHandlers;

public class GetCourseSchedulesQueryHandler(IMapper mapper,
    ICourseRepository courseRepository, 
    ICourseScheduleRepository courseScheduleRepository) 
    : IQueryHandler<GetCourseSchedulesQuery, IEnumerable<CourseScheduleDto>>
{
    public async Task<IEnumerable<CourseScheduleDto>> Handle(GetCourseSchedulesQuery request, CancellationToken cancellationToken)
    {
        var foundCourse = await courseRepository.Get(course => course.Id == request.CourseId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        if (request.IsActiveOnly && foundCourse.Status != ContentStatus.Activated)
            throw new AuthenticationException("Unauthorized Access!");

        var schedules = courseScheduleRepository.Get(schedule => schedule.CourseId == request.CourseId)
            .Include(schedule => schedule.Instructors)
            .Include(schedule => schedule.Enrollments)
            .Select(schedule => new
            {
                Schedule = schedule,
                Enrollments = schedule.Enrollments == null ? 0 : schedule.Enrollments.Count,
                AllowedEnrollments = foundCourse.EnrollmentsCountPerWeek
            })
            .AsEnumerable() 
            .Select(result => (
                Schedule: result.Schedule,
                IsAvailable: result.Enrollments < result.AllowedEnrollments
            ));

        return mapper.Map<IEnumerable<CourseScheduleDto>>(schedules);
    }
}
