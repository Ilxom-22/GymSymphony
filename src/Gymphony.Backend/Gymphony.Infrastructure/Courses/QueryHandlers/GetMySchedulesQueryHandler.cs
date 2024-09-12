using AutoMapper;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Courses.QueryHandlers;

public class GetMySchedulesQueryHandler(
    IMapper mapper,
    IRequestContextProvider requestContextProvider,
    ICourseScheduleEnrollmentRepository courseScheduleEnrollmentRepository) : IQueryHandler<GetMySchedulesQuery, IEnumerable<MyScheduleDto>>
{
    public async Task<IEnumerable<MyScheduleDto>> Handle(GetMySchedulesQuery request, CancellationToken cancellationToken)
    {
        var memberId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var courseSchedules = await courseScheduleEnrollmentRepository
            .Get(cs => cs.MemberId == memberId, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(cs => cs.CourseSchedule!)
            .ThenInclude(cs => cs.Course!)
            .Include(cs => cs.CourseSchedule!)
            .ThenInclude(cs => cs.Instructors!)
            .ThenInclude(i => i.ProfileImage!)
            .ThenInclude(pi => pi.StorageFile)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<MyScheduleDto>>(courseSchedules);
    }
}
