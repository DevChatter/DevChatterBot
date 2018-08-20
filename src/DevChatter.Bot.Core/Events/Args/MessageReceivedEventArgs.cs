using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Events.Args
{
    public class MessageReceivedEventArgs
    {
        public string MessageText { get; set; }
        public ChatUser ChatUser { get; set; }
        public string RoomId { get; set; }
    }
}
