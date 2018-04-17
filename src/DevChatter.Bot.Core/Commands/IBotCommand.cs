using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public interface IBotCommand
    {
        UserRole RoleRequired { get; }
        string PrimaryCommandText { get; }
        string HelpText { get; }
        string FullHelpText { get; }
        bool ShouldExecute(string commandText);
        void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}
