using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public class BotMain
    {
        private readonly List<IChatClient> _chatClients;

        public BotMain(List<IChatClient> chatClients)
        {
            _chatClients = chatClients;
        }

        public void Run()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();

            PublishMessages(automatedMessagingSystem);

            ConnectChatClients();

            BeginLoop(automatedMessagingSystem);
        }

        private void BeginLoop(AutomatedMessagingSystem automatedMessagingSystem)
        {
            while (true)
            {
                Thread.Sleep(1000);

                automatedMessagingSystem.CheckMessages(DateTime.Now);

                while (automatedMessagingSystem.DequeueMessage(out string theMessage))
                {
                    var message = $"{DateTime.Now.ToShortTimeString()} - {theMessage}";
                    foreach (var chatClient in _chatClients)
                    {
                        chatClient.SendMessage(message);
                    }
                }
            }
        }

        private static void PublishMessages(AutomatedMessagingSystem automatedMessagingSystem)
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage
            {
                DelayInMinutes = 1,
                Message = "Hello, everyone! I am the bot!"
            };
            automatedMessagingSystem.Publish(intervalTriggeredMessage);
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