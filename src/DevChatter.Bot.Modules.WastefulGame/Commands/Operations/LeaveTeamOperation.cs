using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System.Collections.Generic;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class LeaveTeamOperation : BaseGameCommandOperation
    {
        public LeaveTeamOperation(IGameRepository gameRepository)
        {
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "leave", "exit", "escape", "quit" };

        public override string TryToExecute(CommandReceivedEventArgs eventArgs,
            Survivor survivor)
        {
            return "You cannot leave teams yet. Please wait for this feature.";
        }
    }
}
