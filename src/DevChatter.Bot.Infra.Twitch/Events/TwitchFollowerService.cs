using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.FollowerService;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    public class TwitchFollowerService : IFollowerService
    {
        public DateTime StartUpTime { get; set; } = DateTime.UtcNow;
        private readonly ITwitchAPI _twitchApi;
        private readonly TwitchClientSettings _settings;
        private readonly FollowerService _followerService;

        private List<string> _followedUsers;

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

        public async Task<IList<string>> GetUsersWeFollow()
        {
            if (_followedUsers == null)
            {
                GetUsersFollowsResponse getUsersFollowsResponse =
                    await _twitchApi.Helix.Users.GetUsersFollowsAsync(fromId: _settings.TwitchChannelId);
                var followedIds = getUsersFollowsResponse.Follows.Select(x => x.ToUserId).ToList();
                GetUsersResponse getUsersResponse =
                    await _twitchApi.Helix.Users.GetUsersAsync(followedIds);
                _followedUsers = getUsersResponse.Users.Select(x => x.DisplayName).ToList();
            }
            return _followedUsers;
        }

        public event EventHandler<NewFollowersEventArgs> OnNewFollower;
    }
}
