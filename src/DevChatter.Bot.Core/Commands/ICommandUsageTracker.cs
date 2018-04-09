using System;

namespace DevChatter.Bot.Core.Commands
{
    public interface ICommandUsageTracker
    {
        CommandUsage GetByUserDisplayName(string userDisplayName);
        void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime);
        void RecordUsage(CommandUsage commandUsage);
    }
}