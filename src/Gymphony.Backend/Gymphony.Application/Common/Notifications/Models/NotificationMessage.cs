using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Notifications.Models;

public class NotificationMessage
{
    public Guid TemplateId { get; set; }

    public Guid RecipientId { get; set; }

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public NotificationStatus Status { get; set; }

    public bool IsRendered { get; set; }

    public DateTimeOffset? SentTime { get; set; }

    public Dictionary<string, string> Variables { get; set; } = new();
}