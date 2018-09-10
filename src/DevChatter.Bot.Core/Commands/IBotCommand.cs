using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public interface IBotCommand
    {
        UserRole RoleRequired { get; }
        TimeSpan Cooldown { get; }
        string PrimaryCommandText { get; }
        string HelpText { get; }
        string FullHelpText { get; }
        bool ShouldExecute(string commandText, out IList<string> args);
        CommandUsage Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
        TimeSpan GetCooldownTimeRemaining();
        bool IsActiveGame();
    }
}
