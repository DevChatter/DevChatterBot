using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandUsageTracker : ICommandUsageTracker
    {
        private readonly CommandHandlerSettings _settings;

        private readonly IList<CommandUsage> _userCommandUsages = new List<CommandUsage>();

        public CommandUsageTracker(CommandHandlerSettings settings)
        {
            _settings = settings;
        }

        public void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime)
        {
            var expiredCooldowns = new List<CommandUsage>();

            foreach (var cooldownPair in _userCommandUsages)
            {
                var elapsedTime = currentTime - cooldownPair.TimeInvoked;
                if (elapsedTime.TotalSeconds >= _settings.GlobalCommandCooldown)
                {
                    expiredCooldowns.Add(cooldownPair);
                }
            }

            foreach (var user in expiredCooldowns)
            {
                _userCommandUsages.Remove(user);
            }
        }

        public CommandUsage GetByUserDisplayName(string userDisplayName)
        {
            return _userCommandUsages.SingleOrDefault(x => x.DisplayName.EqualsIns(userDisplayName));
        }

        public void RecordUsage(CommandUsage commandUsage)
        {
            _userCommandUsages.Add(commandUsage);
        }
    }
}