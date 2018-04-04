using System;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Startup;

namespace DevChatter.Bot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");
            
            (string connectionString, TwitchClientSettings clientSettings, CommandHandlerSettings commandHandlerSettings) = SetUpConfig.InitializeConfiguration();

            BotMain botMain = SetUpBot.NewBot(clientSettings, commandHandlerSettings, connectionString);
            WaitForCommands(botMain);
        }

        private static void WaitForCommands(BotMain botMain)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("Available bot commands : start, stop, exit");
            Console.WriteLine("==============================");

            var command = "start";
            while (true)
            {
                switch (command)
                {
                    case "stop":
                        Console.WriteLine("Bot stopping....");
                        botMain.Stop();
                        Console.WriteLine("Bot stopped");
                        Console.WriteLine("==============================");
                        break;

                    case "start":
                        Console.WriteLine("Bot starting....");
                        botMain.Run();
                        Console.WriteLine("Bot started");
                        Console.WriteLine("==============================");
                        break;

                    case "exit":
                        return;

                    default:
                    {
                        Console.WriteLine($"{command} is not a valid command");
                        break;
                    }
                }

                Console.Write("Bot:");
                command = Console.ReadLine();
            }
        }
    }
}