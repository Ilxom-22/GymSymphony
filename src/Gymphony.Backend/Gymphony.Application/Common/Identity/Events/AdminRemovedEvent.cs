using Gymphony.Domain.Common.Events;
using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Events;

public class AdminRemovedEvent : EventBase
{
    public Admin RemovedAdmin { get; set; } = default!;

    public Guid RemovedByAdminId { get; set; }
}