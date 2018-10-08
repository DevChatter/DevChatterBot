using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Events.Args
{
    public class NewFollowersEventArgs
    {
        public IList<ChatUser> NewFollowers { get; } = new List<ChatUser>();
    }
}
