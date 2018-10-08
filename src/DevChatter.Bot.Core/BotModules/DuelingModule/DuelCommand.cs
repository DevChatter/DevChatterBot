using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class DuelCommand : BaseCommand
    {
        private readonly List<BaseCommandOperation> _operations
            = new List<BaseCommandOperation>();

        public DuelCommand(IRepository repository,
            IChatUserCollection chatUserCollection,
            DuelingSystem duelingSystem)
            : base(repository)
        {
            _operations.Add(new AcceptChallengeOperation(duelingSystem));
            _operations.Add(new StartChallengeOperation(duelingSystem, chatUserCollection));
        }

        protected override bool HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string firstArg = eventArgs.Arguments.ElementAtOrDefault(0);

            var operationToUse = _operations.SingleOrDefault(
                x => x.ShouldExecute(firstArg.NoAt()));
            if (operationToUse != null)
            {
                string messageToSend = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(messageToSend);
                return true;
            }

            chatClient.SendMessage(HelpText);
            return false;
        }
    }
}
