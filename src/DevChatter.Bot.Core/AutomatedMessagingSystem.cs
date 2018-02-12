using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core
{
    public class AutomatedMessagingSystem
    {
        public List<IAutomatedMessage> ManagedMessages { get; set; } = new List<IAutomatedMessage>();
        public List<string> QueuedMessages { get; set; } = new List<string>();

        public void Publish(IAutomatedMessage automatedMessage)
        {
            ManagedMessages.Add(automatedMessage);
        }

        public void CheckMessages(DateTime currentTime)
        {
            var messagesToQueue = ManagedMessages.Where(m => m.IsItYourTimeToDisplay(currentTime)).Select(m => m.GetMessageInstance(currentTime));

            QueuedMessages.AddRange(messagesToQueue);
        }
    }
}