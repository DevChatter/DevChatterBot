using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DeleteCommandOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;
        private readonly IList<IBotCommand> _allCommands;

        public DeleteCommandOperation(IRepository repository, IList<IBotCommand> allCommands)
        {
            _repository = repository;
            _allCommands = allCommands;
        }

        public override List<string> OperandWords { get; } = new List<string> { "del", "rem", "remove", "delete" };
        public override string HelpText { get; } =
            "Use \"!commands del commandWord\" to delete a command.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            string commandWord = eventArgs?.Arguments?.ElementAtOrDefault(1);

            ChatUser chatUser = eventArgs.ChatUser;
            try
            {
                if (!chatUser.IsInThisRoleOrHigher(UserRole.Mod))
                {
                    return "You need to be a moderator to delete a command.";
                }

                SimpleCommand command = _repository.Single(CommandPolicy.ByCommandText(commandWord));
                if (command == null)
                {
                    return $"I didn't find a !{commandWord} command.";
                }

                IBotCommand botCommand = _allCommands.SingleOrDefault(x => x.ShouldExecute(commandWord));
                if (botCommand != null)
                {
                    _allCommands.Remove(botCommand);
                }
                _repository.Remove(command);
                return $"Removing the !{commandWord} command.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
