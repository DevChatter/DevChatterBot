using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Events
{
    public class UserStatusEventArgs
    {
        public string DisplayName { get; set; }
        public UserRole? Role { get; set; }

        public ChatUser ToChatUser()
        {
            return new ChatUser
            {
                DisplayName = DisplayName,
                Role = Role,
            };
        }
    }
}