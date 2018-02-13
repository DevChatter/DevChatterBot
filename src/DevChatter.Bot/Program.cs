using System;
using System.Threading;
using DevChatter.Bot.Core;

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

            while (true)
            {
                Thread.Sleep(1000);

                automatedMessagingSystem.CheckMessages(DateTime.Now);

                while (automatedMessagingSystem.DequeueMessage(out string theMessage))
                {
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} - {theMessage}");
                }
            }
        }
    }
}
