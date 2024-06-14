using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Notifications.Events;

public class NotificationMessageGeneratedEvent : EventBase
{
    public NotificationMessage Message { get; set; } = default!;
}