using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public class NotificationTemplate : Entity
{
    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public NotificationType Type { get; set; }
}