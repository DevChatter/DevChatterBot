using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevChatter.Bot.Web
{
    public class HangfireAutomationSystem : IAutomatedActionSystem
    {
        private readonly IRepository _repository;
        private readonly IList<IChatClient> _chatClients;

        public HangfireAutomationSystem(IRepository repository, IList<IChatClient> chatClients)
        {
            _repository = repository;
            _chatClients = chatClients;
        }

        public void RunNecessaryActions()
        {
            throw new NotImplementedException();
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
                    BackgroundJob.Schedule(() => InvokeDelayedMessageAction(delayedMessageAction.Message),
                        delayedMessageAction.DelayTimeSpan);
                    break;
                case OneTimeCallBackAction oneTimeCallBackAction:
                    BackgroundJob.Schedule(action, oneTimeCallBackAction.DelayTimeSpan);
                    break;
                case AutomatedMessage automatedMessage:
                    RecurringJob.AddOrUpdate(() => InvokeAutomatedMessage(automatedMessage.Name),
                        Cron.MinuteInterval(automatedMessage.IntervalInMinutes));
                    break;
            }
        }

        public void InvokeDelayedMessageAction(string message)
        {
            foreach (IChatClient chatClient in _chatClients)
            {
                chatClient.SendMessage(message);
            }
        }

        public void InvokeAutomatedMessage(string id)
        {
            var message = _repository.Single(DataItemPolicy<IntervalMessage>.ById(Guid.Parse(id)));
            var automatedMessage = new AutomatedMessage(message.MessageText, message.DelayInMinutes, _chatClients, id);
            automatedMessage.Invoke();
        }

        public void RemoveAction(IIntervalAction actionToRemove)
        {
            // I don't think hangfire needs this...
        }
    }
}
