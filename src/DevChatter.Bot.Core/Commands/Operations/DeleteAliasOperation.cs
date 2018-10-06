using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DeleteAliasOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;
        private readonly ILoggerAdapter<AliasCommand> _logger;

        public DeleteAliasOperation(IRepository repository, ILoggerAdapter<AliasCommand> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override List<string> OperandWords { get; } = new List<string> { "del", "rem", "remove" };
        public override string HelpText { get; } = "Use \"!alias del commandToDelete\" to delete an existing alias.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var word = eventArgs?.Arguments?
                .ElementAtOrDefault(1)?.ToLowerInvariant();
            try
            {
                var commandEntity = _repository.Single(CommandPolicy.ByWord(word));

                if (commandEntity == null)
                {
                    return $"The alias '!{word}' doesn't exist.";
                }

                int numberRemoved = commandEntity.Aliases
                    .RemoveAll(a => a.Word == word);
                if (numberRemoved != 1)
                {
                    return GetErrorMessage(word);
                }
                _repository.Update(commandEntity);

                return $"The alias '!{word}' has been deleted.";
            }
            catch (Exception e)
            {
                _logger.LogError(e, GetErrorMessage(word));
                return GetErrorMessage(word);
            }
        }

        private static string GetErrorMessage(string word)
        {
            return $"Something went wrong when trying to delete the {word} alias.";
        }
    }
}
