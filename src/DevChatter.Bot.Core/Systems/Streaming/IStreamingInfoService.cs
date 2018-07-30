using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IStreamingInfoService
    {
        Task<TimeSpan?> GetUptimeAsync();
        Task<int> GetViewerCountAsync();

    }
}
