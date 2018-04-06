using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public abstract class BaseCommand : IBotCommand
    {
        private readonly bool _isEnabled;
        public UserRole RoleRequired { get; }
        public string PrimaryCommandText => CommandWords.First();
        public IList<string> CommandWords { get; }
        public string HelpText { get; protected set; }

        protected BaseCommand(UserRole roleRequired, params string[] commandWords)
            : this(roleRequired, commandWords, true)
        {
        }
        protected BaseCommand(UserRole roleRequired, IList<string> commandWords, bool isEnabled)
        {
            CommandWords = commandWords;
            RoleRequired = roleRequired;
            _isEnabled = isEnabled;
        }


        public bool ShouldExecute(string commandText) => _isEnabled && CommandWords.Any(x => x.EqualsIns(commandText));

        public abstract void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}