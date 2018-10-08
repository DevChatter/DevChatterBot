using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Systems.Streaming;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.V5.Models.Streams;

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
            return await _twitchApi.V5.Streams.GetUptimeAsync(_twitchClientSettings.TwitchChannelId);
        }

        public async Task<int> GetViewerCountAsync()
        {
            StreamByUser streamByUserAsync = await _twitchApi.V5.Streams.GetStreamByUserAsync(_twitchClientSettings.TwitchChannelId);
            return streamByUserAsync.Stream.Viewers;
        }
    }
}
