using Gymphony.Application.Common.Notifications.Models;

namespace Gymphony.Application.Common.Notifications.Brokers;

public interface IEmailSenderBroker
{
    bool Send(NotificationMessage message, CancellationToken cancellationToken = default);
}