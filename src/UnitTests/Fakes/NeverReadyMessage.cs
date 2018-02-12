using System;
using DevChatter.Bot.Core;

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
    }
}