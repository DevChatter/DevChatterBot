using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using Discord;

namespace DevChatter.Bot.Infra.Discord.Extensions
{
    public static class ModelExtensions
    {
        public static ChatUser ToChatUser(this IGuildUser discordUser, DiscordClientSettings settings)
        {
            var chatUser = new ChatUser
            {
                DisplayName = discordUser.Username,
                Role = discordUser.ToUserRole(settings)
            };

            return chatUser;
        }

        public static UserRole ToUserRole(this IGuildUser discordUser, DiscordClientSettings settings)
        {
            if (discordUser.RoleIds.Any(role => role == settings.DiscordStreamerRoleId))
            {
                return UserRole.Streamer;
            }

            if (discordUser.RoleIds.Any(role => role == settings.DiscordModeratorRoleId))
            {
                return UserRole.Mod;
            }

            if (discordUser.RoleIds.Any(role => role == settings.DiscordSubscriberRoleId))
            {
                return UserRole.Subscriber;
            }

            return UserRole.Everyone;
        }
    }
}
