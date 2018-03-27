using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Data.Model
{
    public class ChatUser : DataEntity
    {
        public string DisplayName { get; set; }
        public UserRole? Role { get; set; }
        public int Tokens { get; set; }

        public bool CanUserRunCommand(IBotCommand botCommand)
        {
            return (Role ?? UserRole.Everyone) <= botCommand.RoleRequired;
        }

    }
}