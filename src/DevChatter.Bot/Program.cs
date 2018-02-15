using System;
using System.Collections.Generic;
using System.Threading;
using DevChatter.Bot.Core;
using DevChatter.Bot.Infra.Twitch;


namespace DevChatter.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");
            Console.WriteLine("To exit, press [Ctrl]+c");
            var automatedMessagingSystem = new AutomatedMessagingSystem();

            var intervalTriggeredMessage = new IntervalTriggeredMessage {DelayInMinutes = 1, Message = "Hello, everyone! I am the bot!"};
            automatedMessagingSystem.Publish(intervalTriggeredMessage);

            List<IChatClient> connectedClients = ConnectChatClients();

            while (true)
            {
                Thread.Sleep(1000);

                automatedMessagingSystem.CheckMessages(DateTime.Now);

                while (automatedMessagingSystem.DequeueMessage(out string theMessage))
                {
                    var message = $"{DateTime.Now.ToShortTimeString()} - {theMessage}";
                    foreach (var chatClient in connectedClients)
                    {
                        chatClient.SendMessage(message);
                    }
                }
            }
        }

        private static List<IChatClient> ConnectChatClients()
        {
            var connectChatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                new TwitchChatClient(),
            };
            Thread.Sleep(2000);
            return connectChatClients;
        }
    }
}
