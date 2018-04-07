namespace DevChatter.Bot.Infra.Discord
{
    public class DiscordClientSettings
    {
        public string DiscordToken { get; set; }
        public ulong DiscordStreamerRoleId { get; set; }
        public ulong DiscordModeratorRoleId { get; set; }
        public ulong DiscordSubscriberRoleId { get; set; }
        public ulong DiscordTextChannelId { get; set; }
        public char CommandPrefix { get; set; }
    }
}