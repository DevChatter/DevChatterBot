namespace DevChatter.Bot.Core.Model
{
    public class ChatUser : DataItem
    {
        public string DisplayName { get; set; }
        public UserRole? Role { get; set; }
        public int Tokens { get; set; }
    }
}