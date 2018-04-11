using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Events.Args
{
    public class CommandReceivedEventArgs
    {
        public string CommandWord { get; set; }
        public IList<string> Arguments { get; set; } = new List<string>();
        public ChatUser ChatUser { get; set; } = new ChatUser();
    }
}
