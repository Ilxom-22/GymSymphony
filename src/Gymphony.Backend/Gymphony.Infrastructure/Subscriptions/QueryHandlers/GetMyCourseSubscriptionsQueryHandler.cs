using AutoMapper;
using Gymphony.Application.Subscriptions.Models.Dtos;
using Gymphony.Application.Subscriptions.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Subscriptions.QueryHandlers;

public class GetMyCourseSubscriptionsQueryHandler(
    IMapper mapper,
    IRequestContextProvider requestContextProvider,
    ICourseSubscriptionRepository courseSubscriptionRepository) 
    : IQueryHandler<GetMyCourseSubscriptionsQuery, IList<CourseSubscriptionDto>>
{
    public async Task<IList<CourseSubscriptionDto>> Handle(GetMyCourseSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var memberId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var courses = await courseSubscriptionRepository.Get(c => c.MemberId == memberId)
            .Include(c => c.LastSubscriptionPeriod!)
            .Include(c => c.Course!)
            .ThenInclude(c => c.CourseImages!)
            .ThenInclude(c => c.StorageFile)
            .Where(c => c.LastSubscriptionPeriod!.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        return mapper.Map<IList<CourseSubscriptionDto>>(courses);
    }
}
