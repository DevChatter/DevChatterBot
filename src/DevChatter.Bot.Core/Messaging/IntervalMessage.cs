using System;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Messaging
{
    public class IntervalMessage : DataEntity, IAutomatedMessage
    {
        private readonly IClock _clock = new SystemClock();

        public IntervalMessage()
        {
        }

        public IntervalMessage(int delayInMinutes, string messageText, IClock clock)
        {
            _clock = clock;
            DelayInMinutes = delayInMinutes;
            MessageText = messageText;
            _previousRunTime = _clock.Now;
        }

        private DateTime _previousRunTime;
        public int DelayInMinutes { get; protected set; }
        public string MessageText { get; protected set; }

        public bool IsTimeToDisplay()
        {
            return _previousRunTime.AddMinutes(DelayInMinutes) <= _clock.Now;
        }

        public string GetMessageInstance()
        {
            _previousRunTime = _clock.Now;
            return MessageText;
        }
    }
}
