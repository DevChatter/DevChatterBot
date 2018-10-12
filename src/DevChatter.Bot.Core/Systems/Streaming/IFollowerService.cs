using DevChatter.Bot.Core.Events.Args;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IFollowerService
    {
        event EventHandler<NewFollowersEventArgs> OnNewFollower;
        Task<IList<string>> GetUsersWeFollow();
    }
}
