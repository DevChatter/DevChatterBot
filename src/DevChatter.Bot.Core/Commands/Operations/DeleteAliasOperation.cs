using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DeleteAliasOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public DeleteAliasOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; } = new List<string>{"del", "rem", "remove"};
        public override string HelpText { get; } = "Use \"!alias del commandToDelete\" to delete an existing alias.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();

            var existingWord = _repository.Single(CommandWordPolicy.ByWord(word));

            if (existingWord == null)
            {
                return $"The command word '!{word}' doesn't exist.";
            }

            if (existingWord.IsPrimary)
            {
                return "The primary command cannot be deleted.";
            }

            _repository.Remove(existingWord);

            return $"The command '!{existingWord.CommandWord}' has been deleted.";
        }
    }
}
