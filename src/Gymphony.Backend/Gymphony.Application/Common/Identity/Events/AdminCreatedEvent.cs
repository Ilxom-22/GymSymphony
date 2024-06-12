using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Events;

public class AdminCreatedEvent : EventBase
{
    public Admin Admin { get; set; } = default!;

    public string TemporaryPassword { get; set; } = default!;
}