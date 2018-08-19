using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedMessage
        : IIntervalAction, IAutomatedItem, IAutomatedMessage, IInterval
    {
        private readonly IRepository _repository;
        private readonly IClock _clock;
        private readonly IList<BufferedMessageSender> _chatClients;
        public int IntervalInMinutes => IntervalTimeSpan.Minutes;
        private readonly IntervalMessage _intervalMessage;
        public string Message => _intervalMessage?.MessageText;
        public TimeSpan IntervalTimeSpan { get; }

        public AutomatedMessage(IntervalMessage intervalMessage,
            IList<BufferedMessageSender> chatClients, IRepository repository)
            : this(intervalMessage, chatClients, repository, new SystemClock())
        {
        }

        public AutomatedMessage(IntervalMessage intervalMessage,
            IList<BufferedMessageSender> chatClients, IRepository repository,
            IClock clock)
        {
            _intervalMessage = intervalMessage;
            IntervalTimeSpan = TimeSpan.FromMinutes(intervalMessage.DelayInMinutes);
            _clock = clock;
            _chatClients = chatClients;
            _nextRunTime = intervalMessage.LastSent.AddMinutes(IntervalInMinutes);
            _repository = repository;
        }

        private DateTime _nextRunTime;

        public bool IsTimeToRun()
        {
            return _nextRunTime <= _clock.UtcNow;
        }

        public void Invoke()
        {
            _nextRunTime = _clock.UtcNow.AddMinutes(IntervalInMinutes);
            foreach (BufferedMessageSender chatClient in _chatClients)
            {
                chatClient.SendMessage(Message);
            }
            _intervalMessage.LastSent = _clock.UtcNow;
            _repository.Update(_intervalMessage);
        }

        public bool IsDone => false;
    }
}
