using System.Collections.Generic;

namespace DevChatter.Bot.Core.Model
{
    public class ChatUser
    {
        public string DisplayName { get; set; }
        public UserRole Role { get; set; }
    }
}