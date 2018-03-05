using System;
using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Infra.Json;
using DevChatter.Bot.Infra.Twitch;
using Microsoft.Extensions.Configuration;

namespace DevChatter.Bot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing the Bot...");
            IConfigurationRoot configuration = InitializeConfiguration();

            FakeData.Initialize();

            var clientSettings = configuration
                .GetSection(nameof(TwitchClientSettings))
                .Get<TwitchClientSettings>();

            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                new TwitchChatClient(clientSettings),
            };

            Console.WriteLine("To exit, press [Ctrl]+c");

            var genericJsonFileRepository = new GenericJsonFileRepository();
            var commandMessages = genericJsonFileRepository.List(new ActiveMessagePolicy<StaticCommandResponseMessage>());
            var commandHandler = new CommandHandler(chatClients, commandMessages);
            var botMain = new BotMain(chatClients, genericJsonFileRepository, commandHandler);
            botMain.Run();
        }

        private static IConfigurationRoot InitializeConfiguration()
        {
            Console.WriteLine("Initializing configuration...");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}