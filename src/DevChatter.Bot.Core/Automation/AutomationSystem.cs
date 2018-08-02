using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomationSystem : IAutomatedActionSystem
    {
        private readonly ILoggerAdapter<AutomationSystem> _logger;
        private readonly IList<IChatClient> _chatClients;
        private readonly List<IIntervalAction> _actions = new List<IIntervalAction>();

        public AutomationSystem(ILoggerAdapter<AutomationSystem> logger, IList<IChatClient> chatClients)
        {
            _logger = logger;
            _chatClients = chatClients;
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            _logger.LogInformation($"Attempting to add, {actionToAdd.Name}.");

            _actions.Add(actionToAdd);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// This is a method used for testing, debugging, and special scenarios only. Not for normal usage.
        /// </summary>
        public void ForceRunAllReadyActions()
        {
            RunAllReadyActions();
        }

        private void RunAllReadyActions()
        {
            var readyActions = _actions.Where(x => x.IsTimeToRun());

            foreach (var action in readyActions)
            {
                action.Invoke();
            }
        }

        // TODO: Make a look that checks for two booleans
            // IsItTimeToRun()
            // IsItReadyToRemove()
    }
}
