using System.Collections.Generic;

namespace DevChatter.Bot.Core.Messaging
{
    public interface IAutomatedMessagingSystem
    {
        IList<IAutomatedMessage> ManagedMessages { get; set; }
        IList<string> QueuedMessages { get; set; }

        void CheckMessages();
        bool DequeueMessage(out string message);
        void Publish(IAutomatedMessage automatedMessage);
    }
}
