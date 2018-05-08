using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using System;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public interface ICommandUsageTracker
    {
        void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime);
        void RecordUsage(CommandUsage commandUsage);
        Cooldown GetActiveCooldown(ChatUser chatUser, IBotCommand botCommand);
    }
}
