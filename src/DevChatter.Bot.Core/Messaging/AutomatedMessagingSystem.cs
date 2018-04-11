using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Messaging
{
    public class AutomatedMessagingSystem : IAutomatedMessagingSystem
    {
        public IList<IAutomatedMessage> ManagedMessages { get; set; } =
            new List<IAutomatedMessage>(); // TODO: Lock down access to this

        public IList<string> QueuedMessages { get; set; } = new List<string>(); // TODO: Lock down access to this

        public void Publish(IAutomatedMessage automatedMessage)
        {
            ManagedMessages.Add(automatedMessage);
        }

        public void CheckMessages()
        {
            var messagesToQueue = ManagedMessages.Where(m => m.IsTimeToDisplay()).Select(m => m.GetMessageInstance());

            QueuedMessages = QueuedMessages.Concat(messagesToQueue).ToList();
        }

        public bool DequeueMessage(out string message)
        {
            message = default(string);
            if (!QueuedMessages.Any()) return false;

            message = QueuedMessages.First();
            QueuedMessages.RemoveAt(0);

            return true;
        }
    }
}
