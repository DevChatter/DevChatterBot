using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using Hangfire;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Web
{
    public class HangfireAutomationSystem : IAutomatedActionSystem
    {
        private readonly ILoggerAdapter<HangfireAutomationSystem> _logger;
        private readonly IList<IChatClient> _chatClients;

        public HangfireAutomationSystem(ILoggerAdapter<HangfireAutomationSystem> logger, IList<IChatClient> chatClients)
        {
            _logger = logger;
            _chatClients = chatClients;
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            _logger.LogInformation($"Attempting to add, {actionToAdd.Name}.");
            switch (actionToAdd)
            {
                case CurrencyUpdate currencyUpdate:
                    RecurringJob.AddOrUpdate(
                        currencyUpdate.Name,
                        currencyUpdate.Action,
                        Cron.MinuteInterval(currencyUpdate.IntervalInMinutes));
                    break;

                case DelayedMessageAction delayedMessageAction:
                    string delayedMessage = delayedMessageAction.Message;
                    BackgroundJob.Schedule(() => _chatClients.Single().SendMessage(delayedMessage), delayedMessageAction.DelayTimeSpan);
                    break;

                case OneTimeCallBackAction oneTimeCallBackAction:
                    BackgroundJob.Schedule(oneTimeCallBackAction.Action, oneTimeCallBackAction.DelayTimeSpan);
                    break;

                case AutomatedMessage automatedMessage:
                    string message = automatedMessage.Message;
                    RecurringJob.AddOrUpdate(
                        automatedMessage.Name,
                        () => _chatClients.Single().SendMessage(message),
                        Cron.MinuteInterval(automatedMessage.IntervalInMinutes));
                    break;
            }
        }
    }
}
