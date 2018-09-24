using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class AddAliasOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public AddAliasOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; } = new List<string> { "add", "new", "create" };
        public override string HelpText { get; } = "Use \"!alias add oldCommand newAlias\" to add an alias to a command.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            if (eventArgs?.Arguments == null
                || eventArgs.Arguments.Count < 3) { return HelpText; }

            var word = eventArgs.Arguments[1].ToLowerInvariant();
            var newAlias = eventArgs.Arguments[2].ToLowerInvariant();
            var arguments = eventArgs.Arguments.Skip(3).ToList();

            var commandEntity = _repository.Single(CommandPolicy.ByWord(word));
            var existingWord = _repository.Single(CommandPolicy.ByWord(newAlias));

            if (string.IsNullOrEmpty(newAlias))
            {
                return "You seem to be missing the new alias you want to set.";
            }

            if (existingWord != null)
            {
                return $"The command word '!{existingWord.CommandWord}' already exists.";
            }

            var alias = new AliasEntity
            {
                Word = newAlias,
                Command = commandEntity,
            };

            for (int i = 0; i < arguments.Count; i++)
            {
                alias.Arguments.Add(new AliasArgumentEntity
                {
                    Argument = arguments[i],
                    Alias = alias,
                    Index = i
                });
            }

            commandEntity.Aliases.Add(alias);

            _repository.Update(commandEntity);

            return $"Created new command alias '!{newAlias}' for '!{word}'.";
        }

    }
}
