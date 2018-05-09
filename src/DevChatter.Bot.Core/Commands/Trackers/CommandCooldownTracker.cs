using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Data.Model;

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

            foreach (var usage in _userCommandUsages)
            {
                var elapsedTime = currentTime - usage.TimeInvoked;
                if (elapsedTime >= usage.CommandUsed.Cooldown)
                {
                    expiredCooldowns.Add(usage);
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

        public List<CommandUsage> GetByCommand(IBotCommand command)
        {
            return _userCommandUsages.Where(x => x.CommandUsed == command).ToList();
        }

        public void RecordUsage(CommandUsage commandUsage)
        {
            _userCommandUsages.Add(commandUsage);
        }

        public List<CommandUsage> GetUsagesByUserSubjectToCooldown(string userDisplayName,
            DateTimeOffset currentTime)
        {
            DateTimeOffset timeStillSubjectedToCooldown = currentTime.AddSeconds(_settings.GlobalCommandCooldown * -1);

            var userCommandUsages = _userCommandUsages
                .Where(x => x.DisplayName.EqualsIns(userDisplayName))
                .Where(IsWithinGlobalCooldown);
            return userCommandUsages.ToList();


            bool IsWithinGlobalCooldown(CommandUsage x) => x.TimeInvoked > timeStillSubjectedToCooldown;
        }

        public Cooldown GetActiveCooldown(ChatUser chatUser, IBotCommand botCommand)
        {
            if (chatUser.IsInThisRoleOrHigher(UserRole.Mod) || botCommand.IsActiveGame())
            {
                return new NoCooldown();
            }

            PurgeExpiredUserCommandCooldowns(DateTimeOffset.UtcNow);

            List<CommandUsage> global = GetUsagesByUserSubjectToCooldown(chatUser.DisplayName, DateTimeOffset.UtcNow);
            if (global != null && global.Any())
            {
                if (!global.Any(x => x.WasUserWarned))
                {
                    global.ForEach(x => x.WasUserWarned = true);
                }

                return new UserCooldown { Message = $"Whoa {chatUser.DisplayName}! Slow down there cowboy!" };
            }

            List<CommandUsage> commandCooldown = GetByCommand(botCommand);
            if (commandCooldown != null && commandCooldown.Any())
            {
                string timeRemaining = botCommand.GetCooldownTimeRemaining().ToExpandingString();
                return new CommandCooldown
                {
                    Message = $"\"{botCommand.PrimaryCommandText}\" is currently on cooldown - Remaining time: {timeRemaining}"
                };
            }

            // TODO: Check for UserCommandCooldown needed.

            return new NoCooldown();
        }
    }
}
