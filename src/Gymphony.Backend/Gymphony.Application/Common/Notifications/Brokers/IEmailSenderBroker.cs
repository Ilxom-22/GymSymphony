using Gymphony.Application.Common.Notifications.Models;
using Gymphony.Application.Common.Notifications.Models.Dtos;

namespace Gymphony.Application.Common.Notifications.Brokers;

public interface IEmailSenderBroker
{
    bool Send(NotificationMessage message, CancellationToken cancellationToken = default);
}