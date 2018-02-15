namespace DevChatter.Bot.Core
{
    public interface IChatClient
    {
        void SendMessage(string message);
    }
}