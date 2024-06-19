using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Notifications.Commands;

public class GetNotificationTemplateByTypeCommand : ICommand<NotificationTemplate?>
{
    public NotificationType TemplateType { get; set; }

    public QueryOptions QueryOptions { get; set; }
}