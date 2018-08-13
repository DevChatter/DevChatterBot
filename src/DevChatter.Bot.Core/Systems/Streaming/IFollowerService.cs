using DevChatter.Bot.Core.Events.Args;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowerService
    {
        event EventHandler<NewFollowersEventArgs> OnNewFollower;
        IList<string> GetUsersWeFollow();
    }
}
