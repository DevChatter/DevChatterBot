using System.Collections.Generic;

namespace DevChatter.Bot.Core
{
    public class AutomatedMessagingSystem
    {
        public List<AutomatedMessage> ManagedMessages { get; set; } = new List<AutomatedMessage>();
        public List<string> QueuedMessages { get; set; } = new List<string>();

        public void Publish(AutomatedMessage automatedMessage)
        {
            ManagedMessages.Add(automatedMessage);
        }
    }
}