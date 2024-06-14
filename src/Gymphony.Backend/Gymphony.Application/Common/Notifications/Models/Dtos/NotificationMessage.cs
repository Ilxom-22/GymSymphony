using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Notifications.Models;

public abstract class NotificationMessage
{
    public NotificationTemplate Template { get; set; } = default!;

    public User Recipient { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public NotificationStatus Status { get; set; }

    public NotificationMethod NotificationMethod { get; set; }

    public bool IsRendered { get; set; }

    public DateTimeOffset? SentTime { get; set; }

    public Dictionary<string, string> Variables { get; set; } = new();
}