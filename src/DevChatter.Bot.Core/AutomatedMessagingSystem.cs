using System.Collections.Generic;

namespace DevChatter.Bot.Core
{
    public class AutomatedMessagingSystem
    {
        public List<IAutomatedMessage> ManagedAutomatedMessages { get; set; } = new List<IAutomatedMessage>();
        public List<string> QueuedMessages { get; set; } = new List<string>();

        public void Publish(IAutomatedMessage automatedMessage)
        {
            ManagedAutomatedMessages.Add(automatedMessage);
        }
    }
}