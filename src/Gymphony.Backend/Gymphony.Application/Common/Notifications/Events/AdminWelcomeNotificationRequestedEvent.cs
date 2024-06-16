using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Notifications.Events;

public class AdminWelcomeNotificationRequestedEvent : EventBase
{
    public Admin Recipient { get; set; } = default!;

    public string TemporaryPassword { get; set; } = default!;
}