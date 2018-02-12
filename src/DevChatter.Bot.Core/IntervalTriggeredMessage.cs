using System;

namespace DevChatter.Bot.Core
{
    public class IntervalTriggeredMessage
    {
        public int DelayInMinutes { get; set; }
        public string Message { get; set; }
    }
}
