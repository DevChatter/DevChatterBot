using System;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Startup;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.Bot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");
            TwitchClientSettings clientSettings = SetUpConfig.InitializeConfiguration();

            DbContextOptions<AppDataContext> options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase("fake-data-db")
                .Options;

            IRepository repository = new EfGenericRepo(new AppDataContext(options));

            new FakeData(repository).Initialize();

            BotMain botMain = SetUpBot.NewBot(clientSettings, repository);
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