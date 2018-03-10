using System;
using DevChatter.Bot.Core;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevChatter.Bot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");
            IConfigurationRoot configuration = InitializeConfiguration();

            DbContextOptions<AppDataContext> options = new DbContextOptionsBuilder<AppDataContext>()
                .UseInMemoryDatabase("fake-data-db")
                .Options;

            var efGenericRepo = new EfGenericRepo(new AppDataContext(options));

            new FakeData(efGenericRepo).Initialize();

            var clientSettings = configuration
                .Get<TwitchClientSettings>();

            Console.WriteLine("To exit, press [Ctrl]+c");

            BotMain botMain = SetUpBot.NewBot(clientSettings, efGenericRepo);
            botMain.Run();
        }


        private static IConfigurationRoot InitializeConfiguration()
        {
            Console.WriteLine("Initializing configuration...");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            builder.AddUserSecrets<Program>(); // TODO: Only do this in development

            return builder.Build();
        }
    }
}