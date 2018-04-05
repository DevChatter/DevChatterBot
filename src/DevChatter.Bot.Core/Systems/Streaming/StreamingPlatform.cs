using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public class StreamingPlatform
    {
        private readonly IChatClient _chatClient;
        private readonly IFollowerService _followerService;
        private readonly IStreamingInfoService _streamingInfoService;

        public StreamingPlatform(IChatClient chatClient, IFollowerService followerService, IStreamingInfoService streamingInfoService)
        {
            _chatClient = chatClient;
            _followerService = followerService;
            _streamingInfoService = streamingInfoService;
        }

        public Task<TimeSpan?> GetUptimeAsync()
        {
            return _streamingInfoService.GetUptimeAsync();
        }
    }
}