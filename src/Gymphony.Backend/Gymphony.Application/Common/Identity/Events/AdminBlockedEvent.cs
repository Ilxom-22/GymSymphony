using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Identity.Events;

public class AdminBlockedEvent : EventBase
{
    public Guid BlockedAdminId { get; set; }

    public Guid BlockedByAdminId { get; set; }
}