using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class StartVoteOperation : BaseCommandOperation
    {
        private readonly VotingSystem _votingSystem;

        public StartVoteOperation(VotingSystem votingSystem)
        {
            _votingSystem = votingSystem;
        }

        public override List<string> OperandWords { get; } = new List<string> {"new", "start", "create" };
        public override string HelpText { get; } = "Use \"!vote new \"Question\" \"First Choice\" Choice2 Choice3 Choice4\" to start a new vote.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            if (_votingSystem.IsVoteActive)
            {
                return "There is aready an active vote.";
            }
            ChatUser chatUser = eventArgs.ChatUser;

            if (chatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
                List<string> newVoteArguments = eventArgs.Arguments.Skip(1).ToList();

                return _votingSystem.StartVote(newVoteArguments);
            }
            else
            {
                return "You don't have permission to start a vote.";
            }

        }
    }
}
