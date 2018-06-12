using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Messaging
{
    public class AutomatedMessage : IIntervalAction
    {
        private readonly string _message;
        public readonly int IntervalInMinutes;
        private readonly IClock _clock;
        private readonly IList<IChatClient> _chatClients;

        public AutomatedMessage(string message, int intervalInMinutes,
            IList<IChatClient> chatClients, string name)
            : this(message, intervalInMinutes, chatClients, new SystemClock(), name)
        {
        }

        public AutomatedMessage(string message, int intervalInMinutes,
            IList<IChatClient> chatClients, IClock clock, string name)
        {
            _message = message;
            IntervalInMinutes = intervalInMinutes;
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
    }
}
