using System;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Web
{
    public class BackgroundAutomationSystem : IAutomatedActionSystem
    {
        private readonly ILoggerAdapter<BackgroundAutomationSystem> _logger;
        private readonly IList<IChatClient> _chatClients;

        public BackgroundAutomationSystem(ILoggerAdapter<BackgroundAutomationSystem> logger, IList<IChatClient> chatClients)
        {
            _logger = logger;
            _chatClients = chatClients;
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            _logger.LogInformation($"Attempting to add, {actionToAdd.Name}.");

            throw new NotImplementedException();
        }

        // TODO: Make a look that checks for two booleans
            // IsItTimeToRun()
            // IsItReadyToRemove()
    }
}
