using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowerService
    {
        event EventHandler<NewFollowersEventArgs> OnNewFollower;
        IList<string> GetUsersWeFollow();
    }
}