using System;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Core.Messaging
{
    public class IntervalTriggeredMessage : DataItem, IAutomatedMessage
    {
        public IntervalTriggeredMessage(int delayInMinutes, string message, 
            DataItemStatus dataItemStatus = DataItemStatus.Draft)
        {
            DelayInMinutes = delayInMinutes;
            Message = message;
            DataItemStatus = dataItemStatus;
        }

        private DateTime _previousRunTime;
        public int DelayInMinutes { get; }
        public string Message { get; }

        public void Initialize(DateTime currentTime)
        {
            _previousRunTime = currentTime;
        }

        public bool IsItYourTimeToDisplay(DateTime currentTime)
        {
            return _previousRunTime.AddMinutes(DelayInMinutes) <= currentTime;
        }

        public string GetMessageInstance(DateTime currentTime)
        {
            _previousRunTime = currentTime;
            return Message;
        }
    }
}
