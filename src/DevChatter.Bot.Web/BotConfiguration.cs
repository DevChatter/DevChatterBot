using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Infra.Twitch;

namespace DevChatter.Bot.Web
{
    public class BotConfiguration
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public GoogleCloudSettings GoogleCloudSettings { get; set; }
        public TwitchClientSettings TwitchClientSettings { get; set; }
        public CommandHandlerSettings CommandHandlerSettings { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultDatabase { get; set; }
        public string WastefulGame { get; set; }
    }
}
