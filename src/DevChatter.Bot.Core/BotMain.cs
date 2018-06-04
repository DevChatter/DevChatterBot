using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly IList<IChatClient> _chatClients;
        private readonly IRepository _repository;
        private readonly IAutomatedActionSystem _autoActionSystem;
        private readonly ICommandHandler _commandHandler;
        private readonly SubscriberHandler _subscriberHandler;
        private readonly IFollowableSystem _followableSystem; // This will eventually be a list of these
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private CancellationTokenSource _stopRequestSource;
        private readonly int _refreshInterval = 1000; //the milliseconds the bot waits before checking for new messages

        public BotMain(IList<IChatClient> chatClients, IRepository repository, IFollowableSystem followableSystem,
            IAutomatedActionSystem automatedActionSystem, ICommandHandler commandHandler,
            SubscriberHandler subscriberHandler, IAutomatedActionSystem autoActionSystem)
        {
            _chatClients = chatClients;
            _repository = repository;
            _followableSystem = followableSystem;
            _automatedActionSystem = automatedActionSystem;
            _commandHandler = commandHandler;
            _subscriberHandler = subscriberHandler;
            _autoActionSystem = autoActionSystem;
        }

        public void Run()
        {
            ScheduleAutomatedMessages();

            ConnectChatClients();

            _followableSystem.HandleFollowerNotifications();

            BeginLoop();
        }


        public void Stop()
        {
            StopLoop();

            _followableSystem.StopHandlingNotifications();

            DisconnectChatClients();
        }

        private void StopLoop()
        {
            _stopRequestSource.Cancel();
        }

        private void BeginLoop()
        {
            _stopRequestSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (_stopRequestSource.Token.IsCancellationRequested != true)
                {
                    await Task.Delay(_refreshInterval);
                    _automatedActionSystem.RunNecessaryActions();
                }
            });
        }


        private void ScheduleAutomatedMessages()
        {
            var messages = _repository.List<IntervalMessage>();
            foreach (IntervalMessage message in messages)
            {
                _autoActionSystem.AddAction(new AutomatedMessage(message, _chatClients));
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
