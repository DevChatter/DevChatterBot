using System.Collections.Generic;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CommandReceivedEventArgs
    {
        public string CommandWord { get; set; }
        public List<string> Arguments { get; set; }
        public ChatUser ChatUser { get; set; }
    }
}