using System;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandUsage
    {
        public CommandUsage(string displayName, DateTimeOffset timeInvoked, bool wasUserWarned)
        {
            DisplayName = displayName;
            TimeInvoked = timeInvoked;
            WasUserWarned = wasUserWarned;
        }

        public string DisplayName { get; set; }
        public DateTimeOffset TimeInvoked { get; set; }
        public bool WasUserWarned { get; set; }
    }
}
