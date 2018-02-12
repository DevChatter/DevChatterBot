using System.Collections.Generic;

namespace DevChatter.Bot.Core
{
    public class AutomatedMessagingSystem
    {
        public List<IntervalTriggeredMessage> ManagedMessages { get; set; } = new List<IntervalTriggeredMessage>();
        public List<string> QueuedMessages { get; set; } = new List<string>();

        public void Publish(IntervalTriggeredMessage intervalTriggeredMessage)
        {
            ManagedMessages.Add(intervalTriggeredMessage);
        }
    }
}