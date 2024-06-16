using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Notifications.Events;

public class AdminUnblockedNotificationRequestedEvent : EventBase
{
    public Admin Recipient { get; set; } = default!;
}