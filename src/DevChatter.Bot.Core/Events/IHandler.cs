using DevChatter.Bot.Core.Commands;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public class DomainEventHandler
    {
        private readonly List<Delegate> _handlers = new List<Delegate>();

        public void Subscribe<T>(Action<T> domainEvent) where T : BaseDomainEvent
        {
            _handlers.Add(domainEvent);
        }

        public void Unsubscribe<T>(Action<T> domainEvent) where T : BaseDomainEvent
        {
            _handlers.Remove(domainEvent);
        }

        public void Handle<T>(T domainEvent) where T : BaseDomainEvent
        {
            foreach (var handler in _handlers)
            {
                if (handler is Action<T> action)
                {
                    action(domainEvent);
                }
            }
        }
    }

    public interface IDomainEventRaiser
    {
        void RaiseEvent<T>(T domainEvent) where T : BaseDomainEvent;
    }

    public class DomainEventRaiser : IDomainEventRaiser
    {
        private readonly DomainEventHandler _domainEventHandler;

        public DomainEventRaiser(DomainEventHandler domainEventHandler)
        {
            _domainEventHandler = domainEventHandler;
        }

        public void RaiseEvent<T>(T domainEvent) where T : BaseDomainEvent
        {
            _domainEventHandler.Handle(domainEvent);
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
