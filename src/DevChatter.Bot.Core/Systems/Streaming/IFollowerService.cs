using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowerService
    {
        event EventHandler<NewFollowersEventArgs> OnNewFollower;
        List<string> GetUsersWeFollow();
    }
}