using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Streaming;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly List<IChatClient> _chatClients;
        private readonly IRepository _repository;
        private readonly AutomatedMessagingSystem _autoMsgSystem = new AutomatedMessagingSystem();
        private readonly CommandHandler _commandHandler;
        private readonly SubscriberHandler _subscriberHandler;
        private readonly IFollowerService _followerService;

        public BotMain(List<IChatClient> chatClients, IRepository repository, CommandHandler commandHandler, SubscriberHandler subscriberHandler, IFollowerService followerService)
        {
            _chatClients = chatClients;
            _repository = repository;
            _commandHandler = commandHandler;
            _subscriberHandler = subscriberHandler;
            _followerService = followerService;
        }

        public void Run()
        {
            PublishMessages();

            ConnectChatClients();

            BeginLoop();
        }

        private void BeginLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);

                _autoMsgSystem.CheckMessages(DateTime.Now);

                while (_autoMsgSystem.DequeueMessage(out string theMessage))
                {
                    var message = $"{DateTime.Now.ToShortTimeString()} - {theMessage}";
                    foreach (var chatClient in _chatClients)
                    {
                        chatClient.SendMessage(message);
                    }
                }
            }
        }

        // TODO: Fix this method's name
        private void PublishMessages()
        {
            var messages = _repository.List(DataItemPolicy<IntervalTriggeredMessage>.ActiveOnly());
            foreach (var message in messages)
            {
                _autoMsgSystem.Publish(message);
            }
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