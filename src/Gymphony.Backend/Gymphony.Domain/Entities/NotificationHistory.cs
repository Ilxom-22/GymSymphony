using Gymphony.Domain.Common.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Domain.Entities;

public class NotificationHistory : AuditableEntity
{
    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public Guid RecipientId { get; set; }

    public Guid TemplateId { get; set; }

    public NotificationStatus Status { get; set; }

    public NotificationMethod NotificationMethod { get; set; }

    public DateTimeOffset? SentDate { get; set; }

    public string? ErrorMessage { get; set; }

    public NotificationTemplate? Template { get; set; }
}