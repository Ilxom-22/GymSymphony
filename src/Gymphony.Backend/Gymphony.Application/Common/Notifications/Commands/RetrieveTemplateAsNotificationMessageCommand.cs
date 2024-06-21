using Gymphony.Application.Common.Notifications.Models.Dtos;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Notifications.Commands;

public class RetrieveTemplateAsNotificationMessageCommand : ICommand<NotificationMessage>
{
    public NotificationType TemplateType { get; set; }
}