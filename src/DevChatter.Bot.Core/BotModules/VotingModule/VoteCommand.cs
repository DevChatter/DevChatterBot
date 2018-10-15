using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class VoteCommand : BaseCommand
    {
        private readonly VotingSystem _votingSystem;
        private readonly IAutomatedActionSystem _automatedActionSystem;

        private readonly List<string> _endWords = new List<string> { "stop", "end", "count", "complete", "finish", "fin" }; 

        private readonly List<BaseCommandOperation> _operations
            = new List<BaseCommandOperation>();

        private OneTimeCallBackAction _endVoteCallback;

        public VoteCommand(IRepository repository, VotingSystem votingSystem, IAutomatedActionSystem automatedActionSystem)
            : base(repository)
        {
            _votingSystem = votingSystem;
            _automatedActionSystem = automatedActionSystem;
            _operations.Add(new StartVoteOperation(votingSystem));
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
            else if (_endWords.Contains(firstArg?.ToLower()))
            {
                string messageToSend = SetVoteToEnd(chatClient, eventArgs);
                chatClient.SendMessage(messageToSend);
            }
            else
            {
                _votingSystem.ApplyVote(eventArgs.ChatUser, firstArg, chatClient);
            }
        }

        private string SetVoteToEnd(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (!_votingSystem.IsVoteActive)
            {
                return "There's no vote right now...";
            }

            if (eventArgs.ChatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
                string delayArg = eventArgs.Arguments.ElementAtOrDefault(1);

                if (string.IsNullOrWhiteSpace(delayArg))
                {
                    return _votingSystem.EndVoting();
                }

                int secondsDelay = GetSecondsDelay(delayArg);
                if (secondsDelay > 0)
                {
                    _endVoteCallback = new OneTimeCallBackAction(secondsDelay, () => chatClient.SendMessage(_votingSystem.EndVoting()));
                    _automatedActionSystem.AddAction(
                        _endVoteCallback);
                    return $"Vote will end in {secondsDelay} seconds.";
                }
                return "Invalid delay specified";
            }

            return "You don't have permission to end the voting...";
        }

        private int GetSecondsDelay(string delayArg)
        {
            int secondsDelay = 0;
            var regex = new Regex("((?<minutes>\\d+)m)?((?<seconds>\\d+)s)?");
            Match match = regex.Match(delayArg);
            if (match.Success)
            {
                if (match.Groups["seconds"].Success
                    && int.TryParse(match.Groups["seconds"].Value,
                    out int secondsParsed))
                {
                    secondsDelay += secondsParsed;
                }

                if (match.Groups["minutes"].Success
                    && int.TryParse(match.Groups["minutes"].Value,
                    out int minutesParsed))
                {
                    secondsDelay += 60 * minutesParsed;
                }
            }

            return secondsDelay;
        }
    }
}
