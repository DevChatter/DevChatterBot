namespace DevChatter.Bot.Core.Messaging
{
    public interface IAutomatedMessage
    {
        bool IsTimeToDisplay();
        string GetMessageInstance();
    }
}