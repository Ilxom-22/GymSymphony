using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Domain.Common.Events;
using MediatR;

namespace Gymphony.Infrastructure.Common.EventBus.Brokers;

public class EventBusBroker(IPublisher mediator) : IEventBusBroker
{
    public ValueTask PublishLocalAsync<TEvent>(TEvent @event) where TEvent : EventBase
    {
        return new ValueTask(mediator.Publish(@event));
    }
}