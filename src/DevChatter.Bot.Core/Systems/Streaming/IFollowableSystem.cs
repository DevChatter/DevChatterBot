namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowableSystem
    {
        void HandleFollowerNotifications();
        void StopHandlingNotifications();
    }
}
