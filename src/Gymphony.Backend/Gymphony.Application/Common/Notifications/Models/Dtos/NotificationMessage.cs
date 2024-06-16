using System.Text;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Notifications.Models.Dtos;

public class NotificationMessage
{
    public Guid TemplateId { get; set; }

    public User Recipient { get; set; } = default!;

    public StringBuilder Title { get; set; } = default!;

    public StringBuilder Content { get; set; } = default!;

    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

    public NotificationMethod NotificationMethod { get; set; }

    public bool IsRendered { get; set; }

    public DateTimeOffset SentTime { get; set; }

    public string? ErrorMessage { get; set; }

    public Dictionary<string, string> Variables { get; set; } = new();
}