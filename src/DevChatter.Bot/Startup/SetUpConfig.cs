using System;
using DevChatter.Bot.Infra.Twitch;
using Microsoft.Extensions.Configuration;

namespace DevChatter.Bot.Startup
{
    public static class SetUpConfig
    {
        public static TwitchClientSettings InitializeConfiguration()
        {
            Console.WriteLine("Initializing configuration...");

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            builder.AddUserSecrets<Program>(); // TODO: Only do this in development

            IConfigurationRoot configuration = builder.Build();

            return configuration.Get<TwitchClientSettings>();
        }
    }
}