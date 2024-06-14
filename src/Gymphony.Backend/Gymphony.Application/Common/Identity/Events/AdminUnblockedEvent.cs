using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.Identity.Events;

public class AdminUnblockedEvent : EventBase
{
    public Guid UnblockedAdminId { get; set; }

    public Guid UnblockedByAdminId { get; set; }
}