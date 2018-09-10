using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Discord;
using DevChatter.Bot.Infra.Twitch;

namespace DevChatter.Bot.Web
{
    public class BotConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public GoogleCloudSettings GoogleCloudSettings { get; set; }
        public TwitchClientSettings TwitchClientSettings { get; set; }
        public DiscordClientSettings DiscordClientSettings { get; set; }
        public CommandHandlerSettings CommandHandlerSettings { get; set; }
    }
}
