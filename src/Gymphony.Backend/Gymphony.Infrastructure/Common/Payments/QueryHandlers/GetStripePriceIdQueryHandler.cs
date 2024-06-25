using AutoMapper;
using Gymphony.Application.Common.Payments.Queries;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Payments.QueryHandlers;

public class GetStripePriceIdQueryHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository) 
    : IQueryHandler<GetStripePriceIdQuery, string>
{
    public async Task<string> Handle(GetStripePriceIdQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductType == ProductType.MembershipPlan)
        {
            var membershipPlan = await membershipPlanRepository
                .Get(plan => plan.Id == request.ProductId)
                .Include(plan => plan.StripeDetails)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ArgumentException($"Product with id {request.ProductId} does not exist!");

            if (membershipPlan.StripeDetails is null)
                throw new ArgumentException($"Product with id {request.ProductId} is not available for purchasing!");
            
            return membershipPlan.StripeDetails.PriceId;
        }

        // Need to adjust the logic of the query handler for querying class memberships when they are added.
        throw new ArgumentException("Unsupported Product Type!");
    }
}