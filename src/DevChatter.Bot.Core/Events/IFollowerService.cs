using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public interface IFollowerService
    {
        event EventHandler<NewFollowersEventArgs> OnNewFollower;
        List<string> GetUsersWeFollow();
    }
}