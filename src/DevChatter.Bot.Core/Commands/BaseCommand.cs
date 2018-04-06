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
        public string CommandText { get; }
        public string HelpText { get; protected set; }

        protected BaseCommand(string commandText, UserRole roleRequired, bool isEnabled = true)
        {
            CommandText = commandText;
            RoleRequired = roleRequired;
            _isEnabled = isEnabled;
        }


        public bool ShouldExecute(string commandText) => _isEnabled && CommandText.EqualsIns(commandText);

        public abstract void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}