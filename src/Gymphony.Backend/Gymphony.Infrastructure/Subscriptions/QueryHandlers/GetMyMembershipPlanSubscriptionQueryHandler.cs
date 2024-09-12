using AutoMapper;
using Gymphony.Application.Subscriptions.Models.Dtos;
using Gymphony.Application.Subscriptions.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Subscriptions.QueryHandlers;

public class GetMyMembershipPlanSubscriptionQueryHandler(
    IMapper mapper,
    IRequestContextProvider requestContextProvider,
    IMembershipPlanSubscriptionRepository membershipPlanSubscriptionRepository)
    : IQueryHandler<GetMyMembershipPlanSubscriptionQuery, MembershipPlanSubscriptionDto?>
{
    public async Task<MembershipPlanSubscriptionDto?> Handle(GetMyMembershipPlanSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var memberId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var subscription = await membershipPlanSubscriptionRepository
            .Get(s => s.MemberId == memberId)
            .Include(s => s.LastSubscriptionPeriod)
            .Include(s => s.MembershipPlan)
            .FirstOrDefaultAsync(s => s.LastSubscriptionPeriod!.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken: cancellationToken);

        if (subscription is null) 
            return null;

        return mapper.Map<MembershipPlanSubscriptionDto>(subscription);
    }
}
