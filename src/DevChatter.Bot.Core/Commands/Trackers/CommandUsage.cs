using System;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandUsage
    {
        public CommandUsage(string displayName, DateTimeOffset timeInvoked, IBotCommand commandUsed)
        {
            DisplayName = displayName;
            TimeInvoked = timeInvoked;
            CommandUsed = commandUsed;
        }

        public string DisplayName { get; }
        public DateTimeOffset TimeInvoked { get; }
        public IBotCommand CommandUsed { get; }
        public bool WasUserWarned { get; set; } = false;
    }
}
