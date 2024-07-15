using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gymphony.Infrastructure.Products.Services;

public class ProductStatusUpdaterBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.UtcNow;
        var nextRunTime = now.Date.AddDays(1).AddHours(1);
        var delay = nextRunTime - now;

        await Task.Delay(delay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdatePendingProductActivations(stoppingToken);
            await DeactivateProducts(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async ValueTask UpdatePendingProductActivations(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var productRepository = scope.ServiceProvider
            .GetRequiredService<IProductRepository>();

        var activationPendingProducts = await productRepository.Get(product =>
                product.Status == ContentStatus.Published &&
                product.ActivationDate == DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        foreach (var product in activationPendingProducts)
        {
            product.Status = ContentStatus.Activated;
            await productRepository.UpdateAsync(product, cancellationToken: cancellationToken);
        }
    }

    private async ValueTask DeactivateProducts(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        var deactivationPendingProducts = await productRepository.Get(product =>
                product.Status == ContentStatus.DeactivationRequested &&
                product.DeactivationDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        foreach (var product in deactivationPendingProducts)
        {
            product.Status = ContentStatus.Deactivated;
            await productRepository.UpdateAsync(product, cancellationToken: cancellationToken);
        }
    }
}