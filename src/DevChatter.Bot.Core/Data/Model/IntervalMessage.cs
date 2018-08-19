using System;

namespace DevChatter.Bot.Core.Data.Model
{
    public class IntervalMessage : DataEntity
    {
        public IntervalMessage()
        {
        }

        public IntervalMessage(int delayInMinutes, string messageText)
        {
            DelayInMinutes = delayInMinutes;
            MessageText = messageText;
        }

        public int DelayInMinutes { get; protected set; }
        public string MessageText { get; protected set; }

        public DateTime LastSent { get; set; } = DateTime.UtcNow;
    }
}
