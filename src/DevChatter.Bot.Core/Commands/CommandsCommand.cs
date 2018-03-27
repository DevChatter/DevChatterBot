using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandsCommand : SimpleCommand
    {
        private readonly List<IBotCommand> _allCommands;

        public CommandsCommand(List<IBotCommand> allCommands)
        {
            _allCommands = allCommands;
            CommandText = "commands";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var listOfCommands = _allCommands.Where(x => eventArgs.ChatUser.CanUserRunCommand(x)).Select(x => $"!{x.CommandText}").ToList();

            string stringOfCommands = string.Join(", ", listOfCommands);
            chatClient.SendMessage($"These are the commands that {eventArgs.ChatUser.DisplayName} is allowed to run: ({stringOfCommands})");
        }
    }
}