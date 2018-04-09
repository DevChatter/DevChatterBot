using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IStreamingPlatform
    {
        Task<TimeSpan?> GetUptimeAsync();
    }
}