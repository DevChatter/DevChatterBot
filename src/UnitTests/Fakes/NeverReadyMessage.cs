using System;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;

namespace UnitTests.Fakes
{
    public class NeverReadyMessage : IAutomatedMessage
    {
        public void Initialize(DateTime currentTime)
        {
        }

        public bool IsItYourTimeToDisplay(DateTime currentTime)
        {
            return false;
        }

        public string GetMessageInstance(DateTime currentTime)
        {
            throw new NotImplementedException("How did you do this?!?!");
        }

        public DataItemStatus DataItemStatus => DataItemStatus.Active;
    }
}