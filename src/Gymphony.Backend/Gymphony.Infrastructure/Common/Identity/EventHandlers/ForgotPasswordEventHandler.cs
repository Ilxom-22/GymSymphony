using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gymphony.Infrastructure.Common.Identity.EventHandlers;

public class ForgotPasswordEventHandler(
    IServiceProvider serviceProvider, 
    IMediator mediator) 
    : IEventHandler<ForgotPasswordEvent>
{
    public async Task Handle(ForgotPasswordEvent request, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        var user = await userRepository.GetByEmailAddressAsync(request.EmailAddress, new QueryOptions(QueryTrackingMode.AsNoTracking))
            ?? throw new ArgumentException($"User with email address {request.EmailAddress} does not exist!");

        await mediator.Publish(new PasswordResetNotificationRequestedEvent { Recipient = user }, cancellationToken);
    }
}