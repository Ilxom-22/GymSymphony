using AutoMapper;
using Gymphony.Application.MembershipPlans.Commands;
using Gymphony.Application.MembershipPlans.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.MembershipPlans.CommandHandlers;

public class DeactivateMembershipPlanCommandHandler(IMapper mapper,
    IMembershipPlanRepository membershipPlanRepository)
    : ICommandHandler<DeactivateMembershipPlanCommand, MembershipPlanDto>
{
    public async Task<MembershipPlanDto> Handle(DeactivateMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var membershipPlan = await membershipPlanRepository
                 .Get(plan => plan.Id == request.MembershipPlanId)
                 .Include(plan => plan.Subscriptions!)
                 .ThenInclude(subscription => subscription.LastSubscriptionPeriod)
                 .FirstOrDefaultAsync(cancellationToken)
             ?? throw new ArgumentException($"Membership Plan with Id {request.MembershipPlanId} does not exist!");

        if (membershipPlan.Subscriptions is null || membershipPlan.Subscriptions.Count == 0)
        {
            membershipPlan.Status = ContentStatus.Draft;
            membershipPlan.ActivationDate = null;
        }
        else
        {
            var subscriptionWithMaxExpiryDate = membershipPlan.Subscriptions
                .Where(s => s.LastSubscriptionPeriod?.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
                .DefaultIfEmpty()
                .Max();

            if (subscriptionWithMaxExpiryDate is null)
            {
                membershipPlan.Status = ContentStatus.Deactivated;
                membershipPlan.DeactivationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }
            else
            {
                membershipPlan.Status = ContentStatus.DeactivationRequested;
                membershipPlan.DeactivationDate = subscriptionWithMaxExpiryDate.LastSubscriptionPeriod!.ExpiryDate;
            }
        }
        
        await membershipPlanRepository.UpdateAsync(membershipPlan, cancellationToken: cancellationToken);

        return mapper.Map<MembershipPlanDto>(membershipPlan);
    }
}