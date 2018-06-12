using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Messaging;
using Hangfire;

namespace DevChatter.Bot.Web
{
    public class HangfireAutomationSystem : IAutomatedActionSystem
    {
        public HangfireAutomationSystem()
        {
        }

        public void RunNecessaryActions()
        {
            // Hangfire does this for us.
        }

        public void AddAction(IIntervalAction actionToAdd)
        {
            Expression<Action> action = () => actionToAdd.Invoke();

            switch (actionToAdd)
            {
                case CurrencyUpdate currencyUpdate:
                    RecurringJob.AddOrUpdate(action, Cron.MinuteInterval(currencyUpdate.IntervalInMinutes));
                    break;
                case DelayedMessageAction delayedMessageAction:
                    BackgroundJob.Schedule(action, delayedMessageAction.DelayTimeSpan);
                    break;
                case OneTimeCallBackAction oneTimeCallBackAction:
                    BackgroundJob.Schedule(action, oneTimeCallBackAction.DelayTimeSpan);
                    break;
                case AutomatedMessage automatedMessage:
                    RecurringJob.AddOrUpdate(action, Cron.MinuteInterval(automatedMessage.Message.DelayInMinutes));
                    break;
            }
        }

        public void RemoveAction(IIntervalAction actionToRemove)
        {
            // I don't think hangfire needs this...
        }
    }
}
