namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowableSystem
    {
        void Connect();
        void Disconnect();
    }
}
