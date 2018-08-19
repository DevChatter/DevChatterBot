using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionSystem
    {
        Task Start();
        void AddAction(IIntervalAction actionToAdd);
    }
}
