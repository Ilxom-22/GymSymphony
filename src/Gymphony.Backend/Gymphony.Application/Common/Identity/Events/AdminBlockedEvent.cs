using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Events;

public class AdminBlockedEvent : EventBase
{
    public Admin BlockedAdmin { get; set; } = default!;
}