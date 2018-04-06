using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandsCommand : BaseCommand
    {
        private readonly List<IBotCommand> _allCommands;

        public CommandsCommand(List<IBotCommand> allCommands)
            : base("commands", UserRole.Everyone)
        {
            _allCommands = allCommands;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var listOfCommands = _allCommands.Where(x => eventArgs.ChatUser.CanUserRunCommand(x)).Select(x => $"!{x.CommandText}").ToList();

            string stringOfCommands = string.Join(", ", listOfCommands);
            chatClient.SendMessage($"These are the commands that {eventArgs.ChatUser.DisplayName} is allowed to run: ({stringOfCommands})");
        }
    }
}