namespace Gymphony.Domain.Common.Events;

public class EventBase : IEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
    
    public bool IsRedelivered { get; set; }
}