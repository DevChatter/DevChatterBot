using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandsCommand : BaseCommand
    {
        public CommandsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
        }

        public IEnumerable<IBotCommand> AllCommands { get; set; }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var listOfCommands = AllCommands.Where(x => eventArgs.ChatUser.CanUserRunCommand(x));
            var listOfDefaultCommands = listOfCommands.Where(x => x.CommandType == "Default").Select(x => $"!{x.PrimaryCommandText}");
            var listOfGameCommands = listOfCommands.Where(x => x.CommandType == "Game").Select(x => $"*!{x.PrimaryCommandText}");
            string stringOfCommands = string.Join(", ", listOfDefaultCommands.Concat(listOfGameCommands));
            chatClient.SendMessage($"These are the commands that {eventArgs.ChatUser.DisplayName} is allowed to run: {stringOfCommands}." +
                $"\n (*Only subscribers can start these games. However, all viewers in chat can join a game once it's started.)");
        }
    }
}
