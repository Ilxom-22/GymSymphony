using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gymphony.Infrastructure.MembershipPlans.Services;

public class MembershipPlanStatusUpdaterBackgroundService(
    IServiceProvider serviceProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.UtcNow;
        var nextRunTime = now.Date.AddDays(1).AddHours(1);
        var delay = nextRunTime - now;
        
        await Task.Delay(delay, stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdatePendingMembershipPlanActivations(stoppingToken);
            await DeactivateMembershipPlans(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async ValueTask UpdatePendingMembershipPlanActivations(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var membershipPlanRepository = scope.ServiceProvider
            .GetRequiredService<IMembershipPlanRepository>();

        var activationPendingPlans = await membershipPlanRepository.Get(plan =>
                plan.Status == ContentStatus.Published &&
                plan.ActivationDate == DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        foreach (var plan in activationPendingPlans)
        {
            plan.Status = ContentStatus.Activated;
            await membershipPlanRepository
                .UpdateAsync(plan, cancellationToken: cancellationToken);
        }
    }

    private async ValueTask DeactivateMembershipPlans(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var membershipPlanRepository = scope.ServiceProvider.GetRequiredService<IMembershipPlanRepository>();

        var deactivationPendingPlans = await membershipPlanRepository.Get(plan =>
                plan.Status == ContentStatus.DeactivationRequested &&
                plan.DeactivationDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        foreach (var plan in deactivationPendingPlans)
        {
            plan.Status = ContentStatus.Deactivated;
            await membershipPlanRepository.UpdateAsync(plan, cancellationToken: cancellationToken);
        }
    }
}