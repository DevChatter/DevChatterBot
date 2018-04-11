using System;
using DevChatter.Bot.Core.Messaging;

namespace UnitTests.Fakes
{
    public class AlwaysReadyMessage : IAutomatedMessage
    {
        public bool IsTimeToDisplay()
        {
            return true;
        }

        public string GetMessageInstance()
        {
            return "Always Ready";
        }
    }
}
