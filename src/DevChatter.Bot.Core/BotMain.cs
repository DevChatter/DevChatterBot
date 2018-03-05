using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly List<IChatClient> _chatClients;
        private readonly IRepository _repository;
        private readonly AutomatedMessagingSystem _autoMsgSystem = new AutomatedMessagingSystem();

        public BotMain(List<IChatClient> chatClients, IRepository repository)
        {
            _chatClients = chatClients;
            _repository = repository;
        }

        public void Run()
        {
            PublishMessages();

            ConnectChatClients();

            WireUpEventHandlers();

            BeginLoop();
        }

        private void WireUpEventHandlers()
        {
            foreach (var chatClient in _chatClients)
            {
                chatClient.OnCommandReceived += CommandReceivedHandler;
            }
        }

        private void CommandReceivedHandler(object sender, CommandReceivedEventArgs e)
        {
            if (sender is IChatClient chatClient)
            {
                switch (e.CommandWord)
                {
                    case "coins":
                        chatClient.SendMessage("Coins?!?! I think you meant points!");
                        break;
                }
            }
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
            var messages = _repository.List<IntervalTriggeredMessage>(new ActiveMessagePolicy());
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