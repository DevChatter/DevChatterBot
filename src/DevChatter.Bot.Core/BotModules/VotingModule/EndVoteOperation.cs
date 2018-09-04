using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class EndVoteOperation : BaseCommandOperation
    {
        private readonly VotingSystem _votingSystem;

        public EndVoteOperation(VotingSystem votingSystem)
        {
            _votingSystem = votingSystem;
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
                return _votingSystem.EndVoting();
            }

            return "You don't have permission to end the voting...";
        }
    }
}
