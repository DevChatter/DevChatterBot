using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Trackers;
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
        public bool IsEnabled { get; private set; }
        public UserRole RoleRequired { get; private set; }
        public TimeSpan Cooldown { get; private set; } = TimeSpan.Zero;
        public string PrimaryCommandText => CommandWords.FirstOrDefault().Word;
        public IList<(string Word, IList<string> Args)> CommandWords { get; private set; }
        public string HelpText { get; private set; }
        public virtual string FullHelpText => HelpText;

        protected BaseCommand(IRepository repository)
        {
            Repository = repository;
            RefreshCommandData();
        }

        private void RefreshCommandData()
        {
            CommandEntity command = Repository
                .Single(CommandPolicy.ByType(GetType())) ?? new CommandEntity();
            var cmdInfo = command.Aliases
                .Select(a => (a.Word, (IList<string>)a.Arguments.OrderBy(arg => arg.Index)
                    .Select(arg => arg.Argument).ToList()))
                .ToList();
            cmdInfo.Insert(0, (command.CommandWord, new List<string>()));

            RoleRequired = command.RequiredRole;
            IsEnabled = command.IsEnabled;
            HelpText = command.HelpText;
            Cooldown = command.Cooldown;
            CommandWords = cmdInfo;
        }

        public void NotifyWordsModified() => RefreshCommandData();

        public bool ShouldExecute(string commandText, out IList<string> args)
        {
            args = new List<string>();
            if (IsEnabled)
            {
                if (CommandWords.Any(x => x.Word.EqualsIns(commandText)))
                {
                    var alias = CommandWords.SingleOrDefault(x => x.Word.EqualsIns(commandText));
                    args = alias.Args ?? args;
                    return true;
                }
            }
            return false;
        }

        public CommandUsage Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            _timeCommandLastInvoked = DateTimeOffset.UtcNow;
            HandleCommand(chatClient, eventArgs);
            return new CommandUsage(eventArgs.ChatUser.DisplayName, DateTimeOffset.UtcNow, this);
        }

        public TimeSpan GetCooldownTimeRemaining()
        {
            TimeSpan timePassedSinceInvoke = DateTimeOffset.UtcNow - _timeCommandLastInvoked;
            return (Cooldown - timePassedSinceInvoke);
        }

        protected abstract void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs);

        public bool IsActiveGame()
        {
            return (this is IGameCommand gameCommand && gameCommand.Game.IsRunning);
        }
    }
}
