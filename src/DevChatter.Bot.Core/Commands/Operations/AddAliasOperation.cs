using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class AddAliasOperation : ICommandOperation
    {
        private readonly IRepository _repository;

        public AddAliasOperation(IRepository repository)
        {
            _repository = repository;
        }

        public bool ShouldExecute(string operand)
        {
            return operand.EqualsIns("add");
        }

        public string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();
            var newAlias = eventArgs?.Arguments?.ElementAtOrDefault(2)?.ToLowerInvariant();
            var typeName = _repository.Single(CommandWordPolicy.ByWord(word))?.FullTypeName;
            var existingWord = _repository.Single(CommandWordPolicy.ByWord(newAlias));

            if (string.IsNullOrEmpty(newAlias))
            {
                return "You seem to be missing the new alias you want to set.";
            }

            if (existingWord != null)
            {
                return $"The command word '!{existingWord.CommandWord}' already exists.";
            }

            var newCommand = new CommandWordEntity
            {
                CommandWord = newAlias,
                FullTypeName = typeName,
                IsPrimary = false
            };

            _repository.Create(newCommand);

            return $"Created new command alias '!{newAlias}' for '!{word}'.";
        }
    }
}
