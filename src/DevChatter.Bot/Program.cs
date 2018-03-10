using System;
using DevChatter.Bot.Core;
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

            var efGenericRepo = new EfGenericRepo(new AppDataContext(options));

            new FakeData(efGenericRepo).Initialize();

            Console.WriteLine("To exit, press [Ctrl]+c");

            BotMain botMain = SetUpBot.NewBot(clientSettings, efGenericRepo);
            botMain.Run();
        }

    }
}