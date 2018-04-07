using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using Discord.WebSocket;

namespace DevChatter.Bot.Infra.Discord.Extensions
{
    public static class ModelExtensions
    {
        public static ChatUser ToChatUser(this SocketGuildUser discordUser, DiscordClientSettings settings)
        {
            var chatUser = new ChatUser
            {
                DisplayName = discordUser.Username,
                Role = discordUser.ToUserRole(settings)
            };

            return chatUser;
        }

        public static UserRole ToUserRole(this SocketGuildUser discordUser, DiscordClientSettings settings)
        {
            if (discordUser.Roles.Any(role => role.Id == settings.DiscordStreamerRoleId))
            {
                return UserRole.Streamer;
            }

            if (discordUser.Roles.Any(role => role.Id == settings.DiscordModeratorRoleId))
            {
                return UserRole.Mod;
            }

            if (discordUser.Roles.Any(role => role.Id == settings.DiscordSubscriberRoleId))
            {
                return UserRole.Subscriber;
            }

            return UserRole.Everyone;
        }
    }
}
