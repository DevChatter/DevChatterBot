using System;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Messaging
{
    public class IntervalTriggeredMessage : DataItem, IAutomatedMessage
    {
        public IntervalTriggeredMessage()
        {
        }

        public IntervalTriggeredMessage(int delayInMinutes, string message, 
            DataItemStatus dataItemStatus = DataItemStatus.Draft)
        {
            DelayInMinutes = delayInMinutes;
            Message = message;
            DataItemStatus = dataItemStatus;
            _previousRunTime = DateTime.Now;
        }

        private DateTime _previousRunTime;
        public int DelayInMinutes { get; }
        public string Message { get; }

        public bool IsTimeToDisplay()
        {
            return _previousRunTime.AddMinutes(DelayInMinutes) <= DateTime.Now;
        }

        public string GetMessageInstance()
        {
            _previousRunTime = DateTime.Now;
            return Message;
        }
    }
}
