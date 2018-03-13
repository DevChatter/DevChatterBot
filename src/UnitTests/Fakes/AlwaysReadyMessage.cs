using System;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;

namespace UnitTests.Fakes
{
    public class AlwaysReadyMessage : IAutomatedMessage
    {
        public void Initialize(DateTime currentTime)
        {
        }

        public bool IsItYourTimeToDisplay(DateTime currentTime)
        {
            return true;
        }

        public string GetMessageInstance(DateTime currentTime)
        {
            return "Always Ready";
        }

        public DataItemStatus DataItemStatus => DataItemStatus.Active;
    }
}