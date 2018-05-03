using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Data.Model
{
    public class ChatUser : DataEntity
    {
        public string DisplayName { get; set; }
        public UserRole? Role { get; set; }
        public int Tokens { get; set; }

        public bool CanRunCommand(IBotCommand botCommand)
        {
            return IsInThisRoleOrHigher(botCommand.RoleRequired);
        }

        public bool IsInThisRoleOrHigher(UserRole userRole)
        {
            return (Role ?? UserRole.Everyone) <= userRole;
        }
    }
}
