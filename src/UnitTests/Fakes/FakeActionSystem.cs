using DevChatter.Bot.Core.Automation;

namespace UnitTests.Fakes
{
    public class FakeActionSystem : IAutomatedActionSystem
    {
        public IIntervalAction IntervalAction { get; set; }

        public void AddAction(IIntervalAction actionToAdd)
        {
            IntervalAction = actionToAdd;
        }

        public void RemoveAction(IIntervalAction actionToRemove)
        {
            throw new System.NotImplementedException();
        }

        public void InvokeAction(string id)
        {
            throw new System.NotImplementedException();
        }

        public void RunNecessaryActions()
        {
            throw new System.NotImplementedException();
        }
    }
}
