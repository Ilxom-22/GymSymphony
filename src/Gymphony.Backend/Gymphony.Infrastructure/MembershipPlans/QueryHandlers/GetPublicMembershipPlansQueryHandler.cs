using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.MembershipPlans.Queries;
using Gymphony.Application.Products.Services;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.QueryHandlers;

public class GetPublicMembershipPlansQueryHandler(
    IProductsMapperService membershipPlanMapperService,
    IMembershipPlanRepository membershipPlanRepository)
    : IQueryHandler<GetPublicMembershipPlansQuery, PublicMembershipPlansStatusDto>
{
    public Task<PublicMembershipPlansStatusDto> Handle(GetPublicMembershipPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = membershipPlanRepository
            .Get(plan => plan.Status == ContentStatus.Activated 
                         || plan.Status == ContentStatus.Published,
            queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking));

        var result = membershipPlanMapperService.MapToPublicGroupedPlans(plans);

        return Task.FromResult(result);
    }
}