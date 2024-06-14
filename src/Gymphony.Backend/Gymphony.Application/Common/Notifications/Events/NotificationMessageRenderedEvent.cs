using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Notifications.Events;

public class NotificationMessageRenderedEvent : EventBase
{
    public NotificationMessage Message { get; set; } = default!;
}