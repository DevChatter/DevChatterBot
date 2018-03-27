using System;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Messaging
{
    public class IntervalMessage : DataEntity, IAutomatedMessage
    {
        public IntervalMessage()
        {
        }

        public IntervalMessage(int delayInMinutes, string messageText)
        {
            DelayInMinutes = delayInMinutes;
            MessageText = messageText;
            _previousRunTime = DateTime.Now;
        }

        private DateTime _previousRunTime;
        public int DelayInMinutes { get; protected set; }
        public string MessageText { get; protected set; }

        public bool IsTimeToDisplay()
        {
            return _previousRunTime.AddMinutes(DelayInMinutes) <= DateTime.Now;
        }

        public string GetMessageInstance()
        {
            _previousRunTime = DateTime.Now;
            return MessageText;
        }
    }
}
