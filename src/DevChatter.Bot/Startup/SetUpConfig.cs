using System;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Twitch;
using Microsoft.Extensions.Configuration;

namespace DevChatter.Bot.Startup
{
    public static class SetUpConfig
    {
        public static (string, TwitchClientSettings, CommandHandlerSettings) InitializeConfiguration()
        {
            Console.WriteLine("Initializing configuration...");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            builder.AddUserSecrets<Program>(); // TODO: Only do this in development

            IConfigurationRoot configuration = builder.Build();

            return (configuration.GetConnectionString("DevChatterBotDb"), configuration.Get<TwitchClientSettings>(), configuration.Get<CommandHandlerSettings>());
        }
    }
}