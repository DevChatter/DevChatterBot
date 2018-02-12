using System;

namespace DevChatter.Bot.Core
{
    public class IntervalTriggeredMessage : IAutomatedMessage
    {
        private DateTime _previousRunTime;
        public int DelayInMinutes { get; set; }
        public string Message { get; set; }
        public void Initialize(DateTime currentTime)
        {
            _previousRunTime = currentTime;
        }

        public bool IsItYourTimeToDisplay(DateTime currentTime)
        {
            return _previousRunTime.AddMinutes(DelayInMinutes) <= currentTime;
        }
    }
}
