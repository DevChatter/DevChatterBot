using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Events.Args
{
    public class MessageReceivedEventArgs
    {
        public string Message { get; set; }
        public string Channel { get; set; }
        public string RoomId { get; set; }
        public ChatUser ChatUser { get; set; } = new ChatUser();
    }
}
