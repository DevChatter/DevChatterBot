using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;
using TwitchLib;
using TwitchLib.Models.API.v5.Users;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    public class TwitchFollowerService : IFollowerService
    {
        private readonly TwitchAPI _twitchApi;
        private readonly TwitchClientSettings _settings;

        public TwitchFollowerService(TwitchAPI twitchApi, TwitchClientSettings settings)
        {
            _twitchApi = twitchApi;
            _settings = settings;
        }

        public List<string> GetUsersWeFollow()
        {
            UserFollows userFollows = _twitchApi.Users.v5.GetUserFollowsAsync(_settings.TwitchUserID).Result;
            return userFollows.Follows.Select(uf => uf.Channel.Name).ToList();
        }

        public event EventHandler<NewFollowersEventArgs> OnNewFollower;
    }
}