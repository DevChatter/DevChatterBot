using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Model;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public interface IGameCommandOperation
    {
        bool ShouldExecute(string operand);
        string TryToExecute(CommandReceivedEventArgs eventArgs, Survivor survivor);
    }
}
