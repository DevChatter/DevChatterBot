using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedMessage
        : IIntervalAction, IAutomatedItem, IAutomatedMessage, IInterval
    {
        private readonly string _message;
        private readonly IClock _clock;
        private readonly IList<IChatClient> _chatClients;
        public int IntervalInMinutes => IntervalTimeSpan.Minutes;
        public string Message { get; }
        public TimeSpan IntervalTimeSpan { get; }

        public AutomatedMessage(string message, int intervalInMinutes,
            IList<IChatClient> chatClients, string name)
            : this(message, intervalInMinutes, chatClients, new SystemClock(), name)
        {
        }

        public AutomatedMessage(string message, int intervalInMinutes,
            IList<IChatClient> chatClients, IClock clock, string name)
        {
            _message = message;
            IntervalTimeSpan = TimeSpan.FromMinutes(intervalInMinutes);
            _clock = clock;
            Name = name;
            _chatClients = chatClients;
            _nextRunTime = _clock.UtcNow.AddMinutes(IntervalInMinutes);
        }

        private DateTime _nextRunTime;

        public string Name { get; }

        public bool IsTimeToRun()
        {
            return _nextRunTime <= _clock.UtcNow;
        }

        public void Invoke()
        {
            _nextRunTime = _clock.UtcNow.AddMinutes(IntervalInMinutes);
            foreach (IChatClient chatClient in _chatClients)
            {
                chatClient.SendMessage(_message);
            }
        }

        public bool IsDone => false;
    }
}
