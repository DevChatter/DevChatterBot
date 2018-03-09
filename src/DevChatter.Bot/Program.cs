using System;
using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
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
                //.GetSection(nameof(TwitchClientSettings))
                .Get<TwitchClientSettings>();

            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                new TwitchChatClient(clientSettings),
            };

            Console.WriteLine("To exit, press [Ctrl]+c");

            var commandMessages = efGenericRepo.List(DataItemPolicy<SimpleResponseMessage>.ActiveOnly());
            var commandHandler = new CommandHandler(chatClients, commandMessages);
            var botMain = new BotMain(chatClients, efGenericRepo, commandHandler);
            botMain.Run();
        }

        private static IConfigurationRoot InitializeConfiguration()
        {
            Console.WriteLine("Initializing configuration...");

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder.AddUserSecrets<Program>(); // TODO: Only do this in development

            return builder.Build();
        }
    }
}