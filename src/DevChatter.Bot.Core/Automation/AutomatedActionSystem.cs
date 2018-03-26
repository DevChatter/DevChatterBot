using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedActionSystem
    {
        private readonly List<IIntervalAction> _actions;

        public AutomatedActionSystem(List<IIntervalAction> actions)
        {
            _actions = actions;
        }

        public void RunNecessaryActions()
        {
            var readyActions = _actions.Where(x => x.IsTimeToRun()).ToList();
            foreach (IIntervalAction readyAction in readyActions)
            {
                readyAction.Invoke();
            }
        }

        public void AddAction(IIntervalAction actionToAdd) => _actions.Add(actionToAdd);
        public void RemoveAction(IIntervalAction actionToRemove) => _actions.Remove(actionToRemove); // Relies on reference equality
    }
}