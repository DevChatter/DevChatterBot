using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class VoteCommand : BaseCommand
    {
        private readonly VotingSystem _votingSystem;

        private readonly List<BaseCommandOperation> _operations
            = new List<BaseCommandOperation>();
        public VoteCommand(IRepository repository, VotingSystem votingSystem, IAutomatedActionSystem automatedActionSystem)
            : base(repository)
        {
            _votingSystem = votingSystem;
            _operations.Add(new StartVoteOperation(votingSystem));
            _operations.Add(new EndVoteOperation(votingSystem, automatedActionSystem));
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string firstArg = eventArgs.Arguments.ElementAtOrDefault(0);

            var operationToUse = _operations.SingleOrDefault(
                x => x.ShouldExecute(firstArg));
            if (operationToUse != null)
            {
                string messageToSend = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(messageToSend);
            }
            else
            {
                _votingSystem.ApplyVote(eventArgs.ChatUser, firstArg, chatClient);
            }
        }
    }
}
