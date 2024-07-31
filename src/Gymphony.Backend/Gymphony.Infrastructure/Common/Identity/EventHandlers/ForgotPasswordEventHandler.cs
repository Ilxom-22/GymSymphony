using Gymphony.Application.Common.Exceptions;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
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

        var user = await userRepository.GetByEmailAddressAsync(request.EmailAddress, new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
            ?? throw new ArgumentException($"User with email address {request.EmailAddress} does not exist!");
        
        if (user.AuthenticationProvider != Provider.EmailPassword)
            throw new InvalidEntityStateChangeException<User>($"Since you signed up using your {user.AuthenticationProvider} account, all the passwords are managed by your provider. Please try to sign in using your {user.AuthenticationProvider} account!");

        await mediator.Publish(new PasswordResetNotificationRequestedEvent { Recipient = user }, cancellationToken);
    }
}