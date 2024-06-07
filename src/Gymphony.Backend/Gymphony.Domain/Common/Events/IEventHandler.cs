using MediatR;

namespace Gymphony.Domain.Common.Events;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : class, INotification
{
    
}