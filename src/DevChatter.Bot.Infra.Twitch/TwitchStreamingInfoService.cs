using DevChatter.Bot.Core.Systems.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api.Helix.Models.Streams;
using TwitchLib.Api.Interfaces;

namespace DevChatter.Bot.Infra.Twitch
{
    public class TwitchStreamingInfoService : IStreamingInfoService
    {
        private readonly ITwitchAPI _twitchApi;
        private readonly TwitchClientSettings _twitchClientSettings;
        private DateTime? _streamStartedAt;

        public TwitchStreamingInfoService(ITwitchAPI twitchApi, TwitchClientSettings twitchClientSettings)
        {
            _twitchApi = twitchApi;
            _twitchClientSettings = twitchClientSettings;
        }

        public async Task<TimeSpan?> GetUptimeAsync()
        {
            if (_streamStartedAt == null || _streamStartedAt < DateTime.UtcNow.AddHours(-1))
            {
                var userIds = new List<string> { _twitchClientSettings.TwitchChannelId };
                GetStreamsResponse streamInfo = await _twitchApi.Helix.Streams.GetStreamsAsync(userIds: userIds);
                _streamStartedAt = streamInfo.Streams.SingleOrDefault()?.StartedAt;
            }
            return DateTime.UtcNow - _streamStartedAt;
        }

        public async Task<int> GetViewerCountAsync()
        {
            var userIds = new List<string>{ _twitchClientSettings.TwitchChannelId };
            GetStreamsResponse streamInfo = await _twitchApi.Helix.Streams.GetStreamsAsync(userIds: userIds);
            return streamInfo.Streams.SingleOrDefault()?.ViewerCount ?? 0;
        }
    }
}
