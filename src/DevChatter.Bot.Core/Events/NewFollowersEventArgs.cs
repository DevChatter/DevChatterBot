using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public class NewFollowersEventArgs
    {
        public List<string> FollowerNames { get; } = new List<string>();
    }
}