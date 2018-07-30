using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Systems.Streaming;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Models.v5.Streams;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchStreamingInfoService : IStreamingInfoService
    {
        private readonly ITwitchAPI _twitchApi;
        private readonly TwitchClientSettings _twitchClientSettings;

        public TwitchStreamingInfoService(ITwitchAPI twitchApi, TwitchClientSettings twitchClientSettings)
        {
            _twitchApi = twitchApi;
            _twitchClientSettings = twitchClientSettings;
        }

        public async Task<TimeSpan?> GetUptimeAsync()
        {
            return await _twitchApi.Streams.v5.GetUptimeAsync(_twitchClientSettings.TwitchChannelId);
        }

        public async Task<int> GetViewerCountAsync()
        {
            StreamByUser streamByUserAsync = await _twitchApi.Streams.v5.GetStreamByUserAsync(_twitchClientSettings.TwitchChannelId);
            return streamByUserAsync.Stream.Viewers;
        }
    }
}
