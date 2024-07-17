using Gymphony.Application.Common.Payments.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Payments.EventHandlers;

public class StripeCheckoutSessionFailedEventHandler(IServiceProvider serviceProvider) : IEventHandler<StripeCheckoutSessionFailedEvent>
{
    public async Task Handle(StripeCheckoutSessionFailedEvent notification, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var pendingEnrollmentsRepository = scope.ServiceProvider.GetRequiredService<IPendingScheduleEnrollmentRepository>();

        await pendingEnrollmentsRepository.BatchDeleteBySessionIdAsync(notification.SessionId, cancellationToken);
    }
}
