using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Messaging
{
    public class AutomatedMessage : IIntervalAction
    {
        public readonly IntervalMessage Message;
        private readonly IClock _clock;
        private readonly IList<IChatClient> _chatClients;

        public AutomatedMessage(IntervalMessage message, IList<IChatClient> chatClients)
            : this(message, chatClients, new SystemClock())
        {
        }

        public AutomatedMessage(IntervalMessage message, IList<IChatClient> chatClients, IClock clock)
        {
            Message = message;
            _clock = clock;
            _chatClients = chatClients;
            _nextRunTime = _clock.UtcNow.AddMinutes(Message.DelayInMinutes);
        }

        private DateTime _nextRunTime;

        public bool IsTimeToRun()
        {
            return _nextRunTime <= _clock.UtcNow;
        }

        public void Invoke()
        {
            _nextRunTime = _clock.UtcNow.AddMinutes(Message.DelayInMinutes);
            foreach (IChatClient chatClient in _chatClients)
            {
                chatClient.SendMessage(Message.MessageText);
            }
        }
    }
}
