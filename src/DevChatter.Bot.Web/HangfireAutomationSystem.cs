using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Util;
using Hangfire;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Web
{
    public class HangfireAutomationSystem : IAutomatedActionSystem
    {
        private readonly ILoggerAdapter<HangfireAutomationSystem> _logger;
        private readonly IList<IChatClient> _chatClients;
        private readonly IDictionary<string, IIntervalAction> _actions;

        public HangfireAutomationSystem(ILoggerAdapter<HangfireAutomationSystem> logger, IList<IChatClient> chatClients)
        {
            _logger = logger;
            _chatClients = chatClients;
            _actions = new ConcurrentDictionary<string, IIntervalAction>();
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            var id = actionToAdd.Name;
            _actions.Add(actionToAdd.Name, actionToAdd);

            // If we use Action e.g. Expression<Action> expression = () => this.InvokeAction(id);
            // Hangfire would create its own instance which we dont want since we need our _actions field
            // so instead we use Action<IAutomatedActionSystem> to tell Hangfire to use DI
            // to resolve the IAutomatedActionSystem parameter from our DI container
            Expression<Action<IAutomatedActionSystem>> expression = x => x.InvokeAction(id);

            switch (actionToAdd)
            {
                case CurrencyUpdate currencyUpdate:
                    RecurringJob.AddOrUpdate(
                        currencyUpdate.Name,
                        expression,
                        Cron.MinuteInterval(currencyUpdate.IntervalInMinutes));
                    break;

                case DelayedMessageAction delayedMessageAction:
                    string delayedMessage = delayedMessageAction.Message;
                    BackgroundJob.Schedule(() => _chatClients.Single().SendMessage(delayedMessage), delayedMessageAction.DelayTimeSpan);
                    break;

                case OneTimeCallBackAction oneTimeCallBackAction:
                    BackgroundJob.Schedule(expression, oneTimeCallBackAction.DelayTimeSpan);
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

        public void InvokeAction(string id)
        {
            if (_actions.TryGetValue(id, out var action))
            {
                action.Invoke();

                if (action is IDelayed)
                {
                    _actions.Remove(id);
                }
            }
            else
            {
                _logger.LogError(null, $"Unable to find automated action with id {id}");
            }
        }
    }
}
