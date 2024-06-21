using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Events;

public class AdminUnblockedEvent : EventBase
{
    public Admin UnblockedAdmin { get; set; } = default!;
}