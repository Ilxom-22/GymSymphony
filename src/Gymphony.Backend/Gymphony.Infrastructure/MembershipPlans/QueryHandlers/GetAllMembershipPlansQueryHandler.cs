using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.MembershipPlans.Queries;
using Gymphony.Application.Products.Services;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.MembershipPlans.QueryHandlers;

public class GetAllMembershipPlansQueryHandler(
    IProductsMapperService productsMapperService,
    IMembershipPlanRepository membershipPlanRepository) 
    : IQueryHandler<GetAllMembershipPlansQuery, MembershipPlansStatusGroupDto>
{
    public Task<MembershipPlansStatusGroupDto> Handle(GetAllMembershipPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = membershipPlanRepository
            .Get(queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking));

        var result = productsMapperService
            .MapToGroupedPlans(plans);

        return Task.FromResult(result);
    }
}