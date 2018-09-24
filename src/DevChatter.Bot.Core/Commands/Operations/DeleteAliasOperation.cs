using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DeleteAliasOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public DeleteAliasOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; } = new List<string> {"del", "rem", "remove"};
        public override string HelpText { get; } = "Use \"!alias del commandToDelete\" to delete an existing alias.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();

            var commandEntity = _repository.Single(CommandPolicy.ByWord(word));

            if (commandEntity == null)
            {
                return $"The command word '!{word}' doesn't exist.";
            }

            int numberRemoved = commandEntity.Aliases.RemoveAll(a => a.Word == word);
            if (numberRemoved != 1)
            {
                return $"Something went wrong when trying to delete {word}.";
            }
            _repository.Update(commandEntity);

            return $"The command '!{commandEntity.CommandWord}' has been deleted.";
        }
    }
}
