using Gymphony.Domain.Common.Events;

namespace Gymphony.Application.Common.EventBus.Brokers;

public interface IEventBusBroker
{
    ValueTask PublishLocalAsync<TEvent>(TEvent @event) where TEvent : EventBase;
}