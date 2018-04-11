using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events.Args
{
    public class NewFollowersEventArgs
    {
        public IList<string> FollowerNames { get; } = new List<string>();
    }
}
