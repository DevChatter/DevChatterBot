using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class AliasCommand : BaseCommand
    {
        private readonly IRepository _repository;

        private readonly List<BaseCommandOperation> _operations;

        public AliasCommand(IRepository repository) : base(repository, UserRole.Mod)
        {
            _repository = repository;
            HelpText = "Use !alias add <existing> <new> to add a new command name, or !alias" +
                       " del <existing> to delete a command name. For example, \"!alias add hangman " +
                       "hm\" creates a new shorthand for Hangman.";
            _operations = new List<BaseCommandOperation>
            {
                new AddAliasOperation(repository),
                new DeleteAliasOperation(repository)
            };
        }

        public EventHandler<EventArgs> CommandAliasModified;

        public override string FullHelpText => "Alias manages aliases for existing commands. "
                                               + string.Join(" ", _operations.Select(x => x.HelpText));

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var oper = eventArgs?.Arguments?.ElementAtOrDefault(0)?.ToLowerInvariant();
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(oper) || string.IsNullOrEmpty(word))
            {
                chatClient.SendMessage(HelpText);
                return;
            }

            var typeName = _repository.Single(CommandWordPolicy.ByWord(word))?.FullTypeName;

            if (typeName == null)
            {
                chatClient.SendMessage($"The command '!{word}' doesn't exist.");
                return;
            }

            var operationToUse = _operations.SingleOrDefault(x => x.ShouldExecute(oper));
            if (operationToUse != null)
            {
                string resultMessage = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(resultMessage);
                CommandAliasModified?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                chatClient.SendMessage(HelpText);
            }
        }
    }
}
