using System;

namespace DevChatter.Bot.Core.Events.Args
{
    public class CommandAliasModifiedEventArgs : EventArgs
    {
        public CommandAliasModifiedEventArgs(string fullTypeName)
        {
            FullTypeName = fullTypeName;
        }
        public string FullTypeName { get; set; }
    }
}
