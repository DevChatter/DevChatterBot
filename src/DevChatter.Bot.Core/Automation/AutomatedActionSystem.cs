using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedActionSystem : IAutomatedActionSystem
    {
        private readonly IList<IIntervalAction> _actions = new List<IIntervalAction>();

        public AutomatedActionSystem()
        {
        }

        public void RunNecessaryActions()
        {
            var readyActions = _actions.Where(x => x.IsTimeToRun()).ToList();
            foreach (IIntervalAction readyAction in readyActions)
            {
                try
                {
                    readyAction.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            _actions.Add(actionToAdd);
        }

        public void RemoveAction(IIntervalAction actionToRemove) =>
            _actions.Remove(actionToRemove); // Relies on reference equality

        public void InvokeAction(string id)
        {
            throw new NotSupportedException();
        }
    }
}
