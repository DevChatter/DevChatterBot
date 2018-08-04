using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly IList<IChatClient> _chatClients;
        private readonly IRepository _repository;
        private readonly ICommandHandler _commandHandler;
        private readonly SubscriberHandler _subscriberHandler;
        private readonly IFollowableSystem _followableSystem; // This will eventually be a list of these
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private bool _areSimpleCommandsSetUp = false;
        private readonly CurrencyUpdate _currencyUpdate;

        public BotMain(IList<IChatClient> chatClients,
            IRepository repository,
            IFollowableSystem followableSystem,
            IAutomatedActionSystem automatedActionSystem,
            ICommandHandler commandHandler,
            SubscriberHandler subscriberHandler,
            CurrencyUpdate currencyUpdate)
        {
            _chatClients = chatClients;
            _repository = repository;
            _followableSystem = followableSystem;
            _automatedActionSystem = automatedActionSystem;
            _commandHandler = commandHandler;
            _subscriberHandler = subscriberHandler;
            _currencyUpdate = currencyUpdate;
        }

        public async Task Run()
        {
            ScheduleAutomatedMessages();

            WireUpCurrencyUpdate();

            ConnectChatClients();

            _followableSystem.HandleFollowerNotifications();

            await _automatedActionSystem.Start();

            await Task.CompletedTask;
        }

        private void WireUpCurrencyUpdate()
        {
            _automatedActionSystem.AddAction(_currencyUpdate);
        }

        public async Task Stop()
        {
            _followableSystem.StopHandlingNotifications();

            await DisconnectChatClients();
        }

        private void ScheduleAutomatedMessages()
        {
            var messages = _repository.List<IntervalMessage>();
            foreach (IntervalMessage message in messages)
            {
                var action = new AutomatedMessage(message.MessageText, message.DelayInMinutes, _chatClients, $"AutomatedMessage-{message.Id}");
                _automatedActionSystem.AddAction(action);
            }
        }

        private async Task DisconnectChatClients()
        {
            foreach (var chatClient in _chatClients)
            {
                chatClient.SendMessage("Goodbye for now! The bot has left the building...");
            }

            var disconnectedTasks = new List<Task>();
            foreach (var chatClient in _chatClients)
            {
                disconnectedTasks.Add(chatClient.Disconnect());
            }
            await Task.WhenAll(disconnectedTasks);
        }

        private async void ConnectChatClients()
        {
            var getUserTasks = new List<Task>();

            foreach (var chatClient in _chatClients)
            {
                getUserTasks.Add(chatClient.Connect());
            }

            await Task.WhenAll(getUserTasks);
        }
    }
}
