using AutoMapper;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Application.MembershipPlans.Queries;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.MembershipPlans.QueryHandlers;

public class GetMembershipPlanDetailsQueryHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository)
    : ICommandHandler<GetMembershipPlanDetailsQuery, MembershipPlanDetailsDto>
{
    public async Task<MembershipPlanDetailsDto> Handle(GetMembershipPlanDetailsQuery request, CancellationToken cancellationToken)
    {
        var planDetails = await membershipPlanRepository.Get(plan => plan.Id == request.MembershipPlanId, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(plan => plan.CreatedBy)
            .Include(plan => plan.ModifiedBy)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Membership plan with id '{request.MembershipPlanId}' does not exist!");

        return mapper.Map<MembershipPlanDetailsDto>(planDetails);
    }
}