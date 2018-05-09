using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch.Extensions;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Models.v5.Users;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.FollowerService;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    public class TwitchFollowerService : IFollowerService
    {
        private readonly ITwitchAPI _twitchApi;
        private readonly TwitchClientSettings _settings;
        private readonly FollowerService _followerService;

        private UserFollow[] _userFollows;

        private IEnumerable<UserFollow> UserFollows => _userFollows
                    ?? (_userFollows = _twitchApi.Users.v5.GetUserFollowsAsync(_settings.TwitchChannelID).Result.Follows);

        public TwitchFollowerService(ITwitchAPI twitchApi, TwitchClientSettings settings)
        {
            _twitchApi = twitchApi;

            _settings = settings;

            _followerService = new FollowerService(twitchApi);

            _followerService.SetChannelByChannelId(settings.TwitchChannelID);

            _followerService.StartService().Wait();

            _followerService.OnNewFollowersDetected += FollowerServiceOnOnNewFollowersDetected;
        }

        private void FollowerServiceOnOnNewFollowersDetected(object sender, OnNewFollowersDetectedArgs eventArgs)
        {
            OnNewFollower?.Invoke(sender, eventArgs.ToNewFollowerEventArgs());
        }

        public IList<string> GetUsersWeFollow()
        {
            return UserFollows.Select(uf => uf.Channel.Name).ToList();
        }

        public event EventHandler<NewFollowersEventArgs> OnNewFollower;
    }
}
