using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IStreamingPlatform : IFollowableSystem, IChatSystem
    {
        Task<TimeSpan?> GetUptimeAsync();
        Task<int> GetViewerCountAsync();
    }
}
