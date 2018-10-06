using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class AliasCommand : BaseCommand
    {
        private readonly ILoggerAdapter<AliasCommand> _logger;
        private readonly List<BaseCommandOperation> _operations;

        public AliasCommand(IRepository repository,
            ILoggerAdapter<AliasCommand> logger)
            : base(repository)
        {
            _logger = logger;
            _operations = new List<BaseCommandOperation>
            {
                new AddAliasOperation(repository),
                new DeleteAliasOperation(repository, logger)
            };
        }

        public EventHandler<CommandAliasModifiedEventArgs> CommandAliasModified;

        public override string FullHelpText => "Alias manages aliases for existing commands. " + string.Join(" ", _operations.Select(x => x.HelpText));

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var oper = eventArgs?.Arguments?.ElementAtOrDefault(0)?.ToLowerInvariant();
            var word = eventArgs?.Arguments?.ElementAtOrDefault(1)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(oper) || string.IsNullOrEmpty(word))
            {
                chatClient.SendMessage(HelpText);
                return;
            }

            var typeName = Repository.Single(CommandPolicy.ByWord(word))?.FullTypeName;

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
                CommandAliasModified?.Invoke(this,
                    new CommandAliasModifiedEventArgs(typeName));
            }
            else
            {
                chatClient.SendMessage(HelpText);
            }
        }
    }
}
