using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public abstract class BaseCommand : IBotCommand
    {
        public UserRole RoleRequired { get; protected set;  }
        public string CommandText { get; protected set; }
        public string HelpText { get; protected set; }
        public bool IsEnabled { get; protected set; }

        public abstract void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}