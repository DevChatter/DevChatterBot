using System;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;

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