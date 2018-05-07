using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandCooldownTracker : ICommandUsageTracker
    {
        private readonly CommandHandlerSettings _settings;

        private readonly List<CommandUsage> _userCommandUsages = new List<CommandUsage>();

        public CommandCooldownTracker(CommandHandlerSettings settings)
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

        public List<CommandUsage> GetByUserDisplayName(string userDisplayName)
        {
            return _userCommandUsages.Where(x => x.DisplayName.EqualsIns(userDisplayName)).ToList();
        }

        public void RecordUsage(CommandUsage commandUsage)
        {
            _userCommandUsages.Add(commandUsage);
        }

        public List<CommandUsage> GetUsagesByUserSubjectToGlobalCooldown(string userDisplayName,
            DateTimeOffset currentTime)
        {
            DateTimeOffset timeStillSubjectedToCooldown = currentTime.AddSeconds(_settings.GlobalCommandCooldown * -1);

            var userCommandUsages = _userCommandUsages
                .Where(x => x.DisplayName.EqualsIns(userDisplayName))
                .Where(IsWithinGlobalCooldown);
            return userCommandUsages.ToList();


            bool IsWithinGlobalCooldown(CommandUsage x) => x.TimeInvoked > timeStillSubjectedToCooldown;
        }
    }
}
