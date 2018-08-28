using DevChatter.Bot.Core.Commands;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public interface IHandler<T>
    {
        void Handle(T domainEvent);
    }

    public class DomainEventHandler<T> : BaseDomainEventHandler
        where T : BaseDomainEvent
    {
        private readonly IHandler<T> _handler;

        public DomainEventHandler(IHandler<T> handler)
        {
            _handler = handler;
        }

        public override void Handle(BaseDomainEvent baseEvent)
        {
            if (baseEvent is T concreteEvent)
            {
                _handler.Handle(concreteEvent);
            }
        }
    }

    public abstract class BaseDomainEventHandler
    {
        public abstract void Handle(BaseDomainEvent baseEvent);
    }

    public interface IDomainEventRaiser
    {
        void RaiseEvent<T>(T domainEvent) where T : BaseDomainEvent;
    }

    public class DomainEventRaiser : IDomainEventRaiser
    {
        private readonly IList<BaseDomainEventHandler> _eventHandlers;

        public DomainEventRaiser(IList<BaseDomainEventHandler> eventHandlers)
        {
            _eventHandlers = eventHandlers;
        }

        public void RaiseEvent<T>(T domainEvent) where T : BaseDomainEvent
        {
            foreach (var handler in _eventHandlers)
            {
                handler.Handle(domainEvent);
            }
        }
    }

    public abstract class BaseDomainEvent
    {
        public DateTime TimeEventRaised { get; set; } = DateTime.UtcNow;
    }

    public class CommandCreatedEvent : BaseDomainEvent
    {
        public CommandCreatedEvent(IBotCommand botCommand)
        {
            BotCommand = botCommand;
        }

        public IBotCommand BotCommand { get; set; }
    }
}
