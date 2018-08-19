using System.Threading.Tasks;
using DevChatter.Bot.Core.Automation;

namespace UnitTests.Fakes
{
    public class FakeActionSystem : IAutomatedActionSystem
    {
        public IIntervalAction IntervalAction { get; set; }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            IntervalAction = actionToAdd;
        }
    }
}
