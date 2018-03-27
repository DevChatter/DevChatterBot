using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CommandReceivedEventArgs
    {
        public string CommandWord { get; set; }
        public List<string> Arguments { get; set; } = new List<string>();
        public ChatUser ChatUser { get; set; } = new ChatUser();
    }
}