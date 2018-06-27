using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Messaging;
using Hangfire;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Web
{
    public class HangfireAutomationSystem : IAutomatedActionSystem
    {
        private readonly ILoggerAdapter<HangfireAutomationSystem> _logger;
        private readonly IDictionary<string, IIntervalAction> _actions;

        public HangfireAutomationSystem(ILoggerAdapter<HangfireAutomationSystem> logger)
        {
            _logger = logger;
            _actions = new ConcurrentDictionary<string, IIntervalAction>();
        }

        public void RunNecessaryActions()
        {
            throw new NotImplementedException();
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
                    RecurringJob.AddOrUpdate(expression, Cron.MinuteInterval(currencyUpdate.IntervalInMinutes));
                    break;

                case DelayedMessageAction delayedMessageAction:
                    BackgroundJob.Schedule(expression, delayedMessageAction.DelayTimeSpan);
                    break;

                case OneTimeCallBackAction oneTimeCallBackAction:
                    BackgroundJob.Schedule(expression, oneTimeCallBackAction.DelayTimeSpan);
                    break;

                case AutomatedMessage automatedMessage:
                    RecurringJob.AddOrUpdate(expression, Cron.MinuteInterval(automatedMessage.IntervalInMinutes));
                    break;
            }
        }

        public void InvokeAction(string id)
        {
            if (_actions.TryGetValue(id, out var action))
            {
                action.Invoke();

                // Remove one time actions which always get queued again by the bot(e.g. messages from games)
                switch (action)
                {
                    case DelayedMessageAction _:
                        _actions.Remove(id);
                        break;

                    case OneTimeCallBackAction _:
                        _actions.Remove(id);
                        break;
                }
            }
            else
            {
                _logger.LogError(null, $"Unable to find automated action with id {id}");
            }
        }

        public void RemoveAction(IIntervalAction actionToRemove)
        {
            // I don't think hangfire needs this...
        }
    }
}
