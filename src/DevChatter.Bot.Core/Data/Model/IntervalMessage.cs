using System;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Data.Model
{
    public class IntervalMessage : DataEntity, IWeightedItem
    {
        public IntervalMessage()
        {
        }

        public IntervalMessage(int delayInMinutes, string messageText, int weight = 1)
        {
            DelayInMinutes = delayInMinutes;
            MessageText = messageText;
            Weight = weight;
        }

        public int DelayInMinutes { get; protected set; }
        public string MessageText { get; protected set; }
        public int Weight { get; protected set; }

        public DateTime LastSent { get; set; } = DateTime.UtcNow;
    }
}
