using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.FollowerService;
using TwitchLib.Api.V5.Models.Users;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    public class TwitchFollowerService : IFollowerService
    {
        public DateTime StartUpTime { get; set; } = DateTime.UtcNow;
        private readonly ITwitchAPI _twitchApi;
        private readonly TwitchClientSettings _settings;
        private readonly FollowerService _followerService;

        private UserFollow[] _userFollows;

        private IEnumerable<UserFollow> UserFollows => _userFollows
                    ?? (_userFollows = _twitchApi.V5.Users.GetUserFollowsAsync(_settings.TwitchChannelId).Result.Follows);

        public TwitchFollowerService(ITwitchAPI twitchApi, TwitchClientSettings settings)
        {
            _twitchApi = twitchApi;

            _settings = settings;

            _followerService = new FollowerService(twitchApi);

            _followerService.SetChannelsById(new List<string> { settings.TwitchChannelId });

            _followerService.Start();

            _followerService.OnNewFollowersDetected += FollowerServiceOnOnNewFollowersDetected;
        }

        private void FollowerServiceOnOnNewFollowersDetected(object sender, OnNewFollowersDetectedArgs eventArgs)
        {
            List<string> newFollowers = eventArgs.NewFollowers
                .Where(f => f.FollowedAt > StartUpTime)
                .Select(x => x.FromUserId)
                .ToList();
            if (newFollowers.Any())
            {
                GetUsersResponse getUsersResponse =
                    _twitchApi.Helix.Users.GetUsersAsync(newFollowers).Result;
                OnNewFollower?.Invoke(sender, getUsersResponse.ToNewFollowerEventArgs());
            }
        }

        public IList<string> GetUsersWeFollow()
        {
            return UserFollows.Select(uf => uf.Channel.Name).ToList();
        }

        public event EventHandler<NewFollowersEventArgs> OnNewFollower;
    }
}
