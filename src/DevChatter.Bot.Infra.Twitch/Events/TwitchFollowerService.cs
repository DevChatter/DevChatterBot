using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Infra.Twitch.Extensions;
using TwitchLib;
using TwitchLib.Events.Services.FollowerService;
using TwitchLib.Models.API.v5.Users;
using TwitchLib.Services;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    public class TwitchFollowerService : IFollowerService
    {
        private readonly TwitchAPI _twitchApi;
        private readonly TwitchClientSettings _settings;
        private readonly FollowerService _followerService;

        private UserFollow[] _userFollows;
        private UserFollow[] UserFollows {
            get
            {
                return _userFollows ?? (_userFollows =
                           _twitchApi.Users.v5.GetUserFollowsAsync(_settings.TwitchUserID).Result.Follows);
            }
        }

        public TwitchFollowerService(TwitchAPI twitchApi, TwitchClientSettings settings)
        {
            _twitchApi = twitchApi;

            _settings = settings;

            _followerService = new FollowerService(twitchApi);

            _followerService.SetChannelByName(settings.TwitchChannel);

            _followerService.StartService().Wait();

            _followerService.OnNewFollowersDetected += FollowerServiceOnOnNewFollowersDetected;
        }

        private void FollowerServiceOnOnNewFollowersDetected(object sender, OnNewFollowersDetectedArgs eventArgs)
        {
            OnNewFollower?.Invoke(sender, eventArgs.ToNewFollowerEventArgs());
        }

        public List<string> GetUsersWeFollow()
        {
            return UserFollows.Select(uf => uf.Channel.Name).ToList();
        }

        public event EventHandler<NewFollowersEventArgs> OnNewFollower;
    }
}