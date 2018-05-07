using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public interface ICommandUsageTracker
    {
        List<CommandUsage> GetByUserDisplayName(string userDisplayName);
        void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime);
        void RecordUsage(CommandUsage commandUsage);
        List<CommandUsage> GetUsagesByUserSubjectToGlobalCooldown(string userDisplayName, DateTimeOffset currentTime);
    }
}
