using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class DomainEventCommand : BaseCommand
    {
        private int _timesEventRaised = 0;
        private readonly IDomainEventRaiser _domainEventRaiser;

        public DomainEventCommand(IRepository repository,
            IDomainEventRaiser domainEventRaiser,
            DomainEventHandler domainEventHandler)
            : base(repository, UserRole.Everyone)
        {
            _domainEventRaiser = domainEventRaiser;
            Action<CommandCreatedEvent> foo = cce => { _timesEventRaised++; };
            domainEventHandler.Subscribe(foo);
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage($"I've been called {_timesEventRaised} times.");

            _domainEventRaiser.RaiseEvent(new CommandCreatedEvent(this));
        }
    }
}
