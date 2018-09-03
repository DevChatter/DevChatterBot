using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly IList<IChatClient> _chatClients;
        private readonly IRepository _repository;
        private readonly ICommandHandler _commandHandler;
        private readonly IList<IStreamingPlatform> _streamingPlatforms;
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private readonly CurrencyUpdate _currencyUpdate;

        public BotMain(IList<IChatClient> chatClients,
            IRepository repository,
            IList<IStreamingPlatform> streamingPlatforms,
            IAutomatedActionSystem automatedActionSystem,
            ICommandHandler commandHandler,
            CurrencyUpdate currencyUpdate)
        {
            _chatClients = chatClients;
            _repository = repository;
            _streamingPlatforms = streamingPlatforms;
            _automatedActionSystem = automatedActionSystem;
            _commandHandler = commandHandler;
            _currencyUpdate = currencyUpdate;
        }

        public async Task Run()
        {
            ScheduleAutomatedMessages();

            WireUpCurrencyUpdate();

            ConnectChatClients();

            foreach (IStreamingPlatform streamingPlatform in _streamingPlatforms)
            {
                streamingPlatform.Connect();
            }

            await _automatedActionSystem.Start();

            await Task.CompletedTask;
        }

        private void WireUpCurrencyUpdate()
        {
            _automatedActionSystem.AddAction(_currencyUpdate);
        }

        public async Task Stop()
        {
            foreach (IStreamingPlatform streamingPlatform in _streamingPlatforms)
            {
                streamingPlatform.Disconnect();
            }

            await DisconnectChatClients();
        }

        private void ScheduleAutomatedMessages()
        {
            var messages = _repository.List<IntervalMessage>();
            // HACK: These need to get wrapped elsewhere...
            var bufferedSenders = _chatClients.Select(c => new BufferedMessageSender(c))
                .ToList();
            foreach (IntervalMessage message in messages)
            {
                var action = new AutomatedMessage(message, bufferedSenders, _repository);
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
