using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedMessage
        : IIntervalAction, IAutomatedItem, IAutomatedMessage, IInterval
    {
        private readonly IClock _clock;
        private readonly IList<IChatClient> _chatClients;
        public int IntervalInMinutes => IntervalTimeSpan.Minutes;
        public string Message { get; }
        public TimeSpan IntervalTimeSpan { get; }

        public AutomatedMessage(string message, int intervalInMinutes,
            IList<IChatClient> chatClients)
            : this(message, intervalInMinutes, chatClients, new SystemClock())
        {
        }

        public AutomatedMessage(string message, int intervalInMinutes,
            IList<IChatClient> chatClients, IClock clock)
        {
            Message = message;
            IntervalTimeSpan = TimeSpan.FromMinutes(intervalInMinutes);
            _clock = clock;
            _chatClients = chatClients;
            _nextRunTime = _clock.UtcNow.AddMinutes(IntervalInMinutes);
        }

        private DateTime _nextRunTime;

        public bool IsTimeToRun()
        {
            return _nextRunTime <= _clock.UtcNow;
        }

        public void Invoke()
        {
            _nextRunTime = _clock.UtcNow.AddMinutes(IntervalInMinutes);
            foreach (IChatClient chatClient in _chatClients)
            {
                chatClient.SendMessage(Message);
            }
        }

        public bool IsDone => false;
    }
}
