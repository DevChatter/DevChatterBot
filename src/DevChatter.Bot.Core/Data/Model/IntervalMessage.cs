using System;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Data.Model
{
    public class IntervalMessage : DataEntity, IWeightedItem
    {
        public IntervalMessage()
        {
        }

        public IntervalMessage(string messageText, int weight = 1)
        {
            MessageText = messageText;
            Weight = weight;
        }

        public string MessageText { get; protected set; }
        public int Weight { get; protected set; }

        public DateTime LastSent { get; set; } = DateTime.UtcNow;
    }
}
