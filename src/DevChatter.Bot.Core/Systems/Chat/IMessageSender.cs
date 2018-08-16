namespace DevChatter.Bot.Core.Systems.Chat
{
    public interface IMessageSender
    {
        void SendMessage(string message);
        void SendDirectMessage(string username, string message);
    }
}
