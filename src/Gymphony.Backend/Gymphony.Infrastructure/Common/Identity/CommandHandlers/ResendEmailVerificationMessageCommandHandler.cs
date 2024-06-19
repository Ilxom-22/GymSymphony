using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Notifications.Events;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class ResendEmailVerificationMessageCommandHandler(
    IEventBusBroker eventBusBroker, IUserRepository userRepository) 
    : ICommandHandler<ResendEmailVerificationMessageCommand, bool>
{
    public async Task<bool> Handle(ResendEmailVerificationMessageCommand request, CancellationToken cancellationToken)
    {
        var foundUser = await userRepository.GetByEmailAddressAsync(request.EmailAddress,
            new QueryOptions(QueryTrackingMode.AsNoTracking), cancellationToken)
            ?? throw new ArgumentException($"User with email address {request.EmailAddress} does not exist!");

        if (foundUser.Status == AccountStatus.Active)
            throw new InvalidOperationException($"User with email address {request.EmailAddress} is already verified!");

        await eventBusBroker.PublishLocalAsync(new EmailVerificationNotificationRequestedEvent { Recipient = foundUser }, cancellationToken);

        return true;
    }
}