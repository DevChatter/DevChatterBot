using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Twitch;

namespace DevChatter.Bot
{
    public class BotConfiguration
    {
        public string DatabaseConnectionString { get; set; }
        public GoogleCloudSettings GoogleCloudSettings { get; set; }
        public TwitchClientSettings TwitchClientSettings { get; set; }
        public CommandHandlerSettings CommandHandlerSettings { get; set; }
    }
}
