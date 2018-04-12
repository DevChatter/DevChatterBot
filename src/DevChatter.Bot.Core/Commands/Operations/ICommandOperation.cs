using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public interface ICommandOperation
    {
        bool ShouldExecute(string operand);
        string TryToExecute(CommandReceivedEventArgs eventArgs);
    }
}
