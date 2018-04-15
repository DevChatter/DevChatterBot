using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public abstract class BaseCommandOperation : ICommandOperation
    {
        public string PrimaryWord => OperandWords.FirstOrDefault();
        public abstract List<string> OperandWords { get; }
        public abstract string HelpText { get; }

        public virtual bool ShouldExecute(string operand)
        {
            return OperandWords.Any(w => w.EqualsIns(operand));
        }

        public abstract string TryToExecute(CommandReceivedEventArgs eventArgs);
    }
}
