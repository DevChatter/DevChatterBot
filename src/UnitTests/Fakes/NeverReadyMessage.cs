using System;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;

namespace UnitTests.Fakes
{
    public class NeverReadyMessage : IAutomatedMessage
    {

        public bool IsTimeToDisplay()
        {
            return false;
        }

        public string GetMessageInstance()
        {
            throw new NotImplementedException("How did you do this?!?!");
        }
    }
}