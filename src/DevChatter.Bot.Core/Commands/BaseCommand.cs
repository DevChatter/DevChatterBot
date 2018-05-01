using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Games;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public abstract class BaseCommand : IBotCommand
    {
        protected readonly IRepository Repository;
        protected DateTimeOffset _timeCommandLastInvoked;
        private readonly bool _isEnabled;
        public UserRole RoleRequired { get; }
        public TimeSpan Cooldown { get; protected set; } = TimeSpan.Zero;
        public string PrimaryCommandText => CommandWords.FirstOrDefault();
        public IList<string> CommandWords { get; private set; }
        public string HelpText { get; protected set; }
        public virtual string FullHelpText => HelpText;

        protected BaseCommand(IRepository repository, UserRole roleRequired)
            : this(repository, roleRequired, true)
        {
        }

        protected BaseCommand(IRepository repository, UserRole roleRequired, bool isEnabled)
        {
            Repository = repository;
            RoleRequired = roleRequired;
            _isEnabled = isEnabled;
            CommandWords = RefreshCommandWords();
        }

        private List<string> RefreshCommandWords()
        {
            return Repository
                       .List(CommandWordPolicy.ByType(GetType()))
                       ?.OrderByDescending(x => x.IsPrimary)
                       .Select(word => word.CommandWord)
                       .ToList() ?? new List<string>();
        }

        public void NotifyWordsModified() => CommandWords = RefreshCommandWords();

        public bool ShouldExecute(string commandText) => _isEnabled && CommandWords.Any(x => x.EqualsIns(commandText));

        public void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            bool userCanBypassCooldown = eventArgs.ChatUser.Role?.EqualsAny(UserRole.Streamer, UserRole.Mod) ?? false;
            bool isGameRunning = false;
            if (this is IGameCommand gameCommand)
            {
                isGameRunning = gameCommand.Game.IsRunning;
            }

            TimeSpan timePassedSinceInvoke = DateTimeOffset.Now - _timeCommandLastInvoked;
            if (isGameRunning || userCanBypassCooldown || timePassedSinceInvoke >= Cooldown)
            {
                _timeCommandLastInvoked = DateTimeOffset.Now;
                HandleCommand(chatClient, eventArgs);
            }
            else
            {
                string timeRemaining = (Cooldown - timePassedSinceInvoke).ToExpandingString();
                string cooldownMessage = $"That command is currently on cooldown - Remaining time: {timeRemaining}";
                chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName, cooldownMessage);
            }
        }

        protected abstract void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}
