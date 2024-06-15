using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Notifications.Events;

public class SystemWelcomeNotificationRequestedEvent : EventBase
{
    public User Recipient { get; set; } = default!;
}