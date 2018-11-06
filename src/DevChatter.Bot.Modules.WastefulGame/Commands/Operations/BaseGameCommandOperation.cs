using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Modules.WastefulGame.Model;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public abstract class BaseGameCommandOperation : IGameCommandOperation
    {
        public abstract List<string> OperandWords { get; }

        public virtual bool ShouldExecute(string operand)
        {
            return OperandWords.Any(w => w.EqualsIns(operand));
        }

        public abstract string TryToExecute(CommandReceivedEventArgs eventArgs, Survivor survivor);
    }
}
