using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DevChatter.Bot.Core.Automation;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class EndVoteOperation : BaseCommandOperation
    {
        private readonly VotingSystem _votingSystem;
        private readonly IAutomatedActionSystem _automatedActionSystem;

        public EndVoteOperation(VotingSystem votingSystem,
            IAutomatedActionSystem automatedActionSystem)
        {
            _votingSystem = votingSystem;
            _automatedActionSystem = automatedActionSystem;
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "stop", "end", "count", "complete", "finish", "fin" };

        public override string HelpText { get; } = "";
        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
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


                // Try parsing it to get the seconds 
                var regex = new Regex("(?<seconds>\\d+)s");
                Match match = regex.Match(delayArg);
                if (match.Success
                    && match.Groups.Count > 0
                    && int.TryParse(match.Groups["seconds"].Value,
                                      out int secondsDelay))
                {
                    _automatedActionSystem.AddAction(
                        new DelayedVoteEnd(secondsDelay, _votingSystem));
                }
                return "Invalid delay specified";
            }

            return "You don't have permission to end the voting...";
        }
    }
}
