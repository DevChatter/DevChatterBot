using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core
{
    public class BotMain : IBotMain
    {
        private readonly IList<IChatClient> _chatClients;
        private readonly IRepository _repository;
        private readonly IAutomatedMessagingSystem _autoMsgSystem = new AutomatedMessagingSystem();
        private readonly ICommandHandler _commandHandler;
        private readonly SubscriberHandler _subscriberHandler;
        private readonly IFollowableSystem _followableSystem; // This will eventually be a list of these
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private CancellationTokenSource _stopRequestSource;
        private readonly int _refreshInterval = 1000;//the milliseconds the bot waits before checking for new messages

        public BotMain(IList<IChatClient> chatClients, IRepository repository, IFollowableSystem followableSystem, IAutomatedActionSystem automatedActionSystem, ICommandHandler commandHandler, SubscriberHandler subscriberHandler)
        {
            _chatClients = chatClients;
            _repository = repository;
            _followableSystem = followableSystem;
            _automatedActionSystem = automatedActionSystem;
            _commandHandler = commandHandler;
            _subscriberHandler = subscriberHandler;
        }

        public void Run()
        {
            PublishMessages();

            ConnectChatClients();

            _followableSystem.HandleFollowerNotifications();

            BeginLoop();
        }


        public void Stop()
        {
            StopLoop();

            _followableSystem.StopHandlingNotifications();

            DisconnectChatClients();

            //todo check if publish messages needs to be cleaned ?
        }

        private void StopLoop()
        {
            _stopRequestSource.Cancel();
        }

        private void BeginLoop()
        {
            _stopRequestSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (_stopRequestSource.Token.IsCancellationRequested != true)
                {
                    Thread.Sleep(_refreshInterval);

                    _automatedActionSystem.RunNecessaryActions();

                    _autoMsgSystem.CheckMessages();

                    while (_autoMsgSystem.DequeueMessage(out string message))
                    {
                        foreach (var chatClient in _chatClients)
                        {
                            chatClient.SendMessage(message);
                        }
                    }
                }
            });
        }

        // TODO: Fix this method's name

        private void PublishMessages()
        {
            var messages = _repository.List<IntervalMessage>();
            foreach (var message in messages)
            {
                _autoMsgSystem.Publish(message);
            }
        }

        private void DisconnectChatClients()
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

            Task.WhenAll(disconnectedTasks);
        }

        private void ConnectChatClients()
        {
            var getUserTasks = new List<Task>();

            foreach (var chatClient in _chatClients)
            {
                getUserTasks.Add(chatClient.Connect());
            }

            Task.WhenAll(getUserTasks);
        }

    }
}