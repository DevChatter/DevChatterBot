using System.Collections.Generic;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class AcceptChallengeOperation : BaseCommandOperation
    {
        private readonly DuelingSystem _duelingSystem;

        public AcceptChallengeOperation(DuelingSystem duelingSystem)
        {
            _duelingSystem = duelingSystem;
        }

        public override List<string> OperandWords { get; }
            = new List<string>{"ok", "accept", "yes", "fight"};
        public override string HelpText { get; } = "";
        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            return "You have accepted the challenge!";
        }
    }
}
