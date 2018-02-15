using TwitchLib.Models.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public static class Hidden
    {
        public static ConnectionCredentials GetCredentials()
        {
            return new ConnectionCredentials("devchatterbot", "");
        }
    }
}