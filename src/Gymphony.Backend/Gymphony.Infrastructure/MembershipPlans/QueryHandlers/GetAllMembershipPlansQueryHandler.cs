using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.MembershipPlans.Queries;
using Gymphony.Application.MembershipPlans.Services;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.QueryHandlers;

public class GetAllMembershipPlansQueryHandler(
    IMembershipPlanMapperService membershipPlanMapperService,
    IMembershipPlanRepository membershipPlanRepository) 
    : IQueryHandler<GetAllMembershipPlansQuery, MembershipPlansStatusGroupDto>
{
    public Task<MembershipPlansStatusGroupDto> Handle(GetAllMembershipPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = membershipPlanRepository
            .Get(queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking));

        var result = membershipPlanMapperService
            .MapToGroupedPlans(plans);

        return Task.FromResult(result);
    }
}