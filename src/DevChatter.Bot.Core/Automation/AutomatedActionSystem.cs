using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedActionSystem : IAutomatedActionSystem
    {
        private readonly IList<IIntervalAction> _actions;
        private readonly IClock _clock;

        public AutomatedActionSystem(IList<IIntervalAction> actions, IClock clock)
        {
            _actions = actions;
            _clock = clock;
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

        public void RemoveAction(IIntervalAction actionToRemove) =>
            _actions.Remove(actionToRemove); // Relies on reference equality
    }
}
