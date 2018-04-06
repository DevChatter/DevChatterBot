using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public interface IBotCommand
    {
        UserRole RoleRequired { get; }
        IList<string> CommandWords { get; }
        string HelpText { get; }
        bool ShouldExecute(string commandText);
        void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}