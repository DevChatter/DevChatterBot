using System;
using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Data.Model
{
    public class ChatUser : DataEntity, IEquatable<ChatUser>
    {
        public string UserId { get; set; }
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

        #region Equality Members

        public bool Equals(ChatUser other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(UserId, other.UserId, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ChatUser)obj);
        }

        public override int GetHashCode()
        {
            return (UserId != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(UserId) : 0);
        }


        #endregion
    }
}
