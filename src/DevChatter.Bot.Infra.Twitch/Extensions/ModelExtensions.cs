using DevChatter.Bot.Core.Data.Model;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;

namespace DevChatter.Bot.Infra.Twitch.Extensions
{
    public static class ModelExtensions
    {
        public static ChatUser ToChatUser(this ChatterFormatted chatterFormatted)
        {
            var chatUser = new ChatUser
            {
                DisplayName = chatterFormatted.Username,
                Role = chatterFormatted.UserType.ToUserRole()
            };
            return chatUser;
        }

        public static UserRole ToUserRole(this UserType userType)
        {
            switch (userType)
            {
                case UserType.Viewer:
                    return UserRole.Everyone;
                case UserType.Moderator:
                    return UserRole.Mod;
                case UserType.GlobalModerator: // Twitch Moderators - not our mods
                    return UserRole.Everyone;
                case UserType.Broadcaster:
                    return UserRole.Streamer;
                case UserType.Admin: // Paid Twitch Moderators - not our mods
                    return UserRole.Everyone;
                case UserType.Staff: // Twitch employees
                    return UserRole.Everyone;
                default:
                    return UserRole.Everyone;
            }
        }
    }
}
