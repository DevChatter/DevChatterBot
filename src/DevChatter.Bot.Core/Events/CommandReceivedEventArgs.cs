using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public class CommandReceivedEventArgs
    {
        public string CommandWord { get; set; }
        public List<string> Arguments { get; set; }
    }
}