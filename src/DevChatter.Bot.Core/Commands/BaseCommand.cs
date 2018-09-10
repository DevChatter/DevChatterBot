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
        private readonly bool _isEnabled;
        public UserRole RoleRequired { get; }
        public TimeSpan Cooldown { get; protected set; } = TimeSpan.Zero;
        public string PrimaryCommandText => CommandWords.FirstOrDefault().Word;
        public IList<(string Word, IList<string> Args)> CommandWords { get; private set; }
        public string HelpText { get; protected set; }
        public virtual string FullHelpText => HelpText;

        protected BaseCommand(IRepository repository, UserRole roleRequired, bool isEnabled = true)
        {
            Repository = repository;
            RoleRequired = roleRequired;
            _isEnabled = isEnabled;
            CommandWords = RefreshCommandWords();
        }

        private List<(string Word, IList<string> Args)> RefreshCommandWords()
        {
            List<CommandWordEntity> commandWordEntities = Repository
                .List(CommandWordPolicy.ByType(GetType())) ?? new List<CommandWordEntity>();
            var words = commandWordEntities
                .OrderByDescending(x => x.IsPrimary)
                .Select(cw => (cw.CommandWord, (IList<string>)cw.Arguments.Select(x => x.Argument).ToList()))
                .ToList();

            return words;
        }

        public void NotifyWordsModified() => CommandWords = RefreshCommandWords();

        public bool ShouldExecute(string commandText, out IList<string> args)
        {
            args = new List<string>();
            if (_isEnabled)
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
