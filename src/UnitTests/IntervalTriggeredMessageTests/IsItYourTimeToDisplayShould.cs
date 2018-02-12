using System;
using DevChatter.Bot.Core;
using Xunit;

namespace UnitTests.IntervalTriggeredMessageTests
{
    public class IsItYourTimeToDisplayShould
    {
        [Fact]
        public void ReturnFalse_AtInitialCreation()
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage { DelayInMinutes = 1, Message = "Hello there!" };
            intervalTriggeredMessage.Initialize(DateTime.Now);

            Assert.False(intervalTriggeredMessage.IsItYourTimeToDisplay(DateTime.Now));
        }

        [Fact]
        public void ReturnTrue_GivenTimeEqualToDelayInMinutes()
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage { DelayInMinutes = 1, Message = "Hello there!" };
            intervalTriggeredMessage.Initialize(DateTime.Now);

            Assert.True(intervalTriggeredMessage.IsItYourTimeToDisplay(DateTime.Now.AddMinutes(intervalTriggeredMessage.DelayInMinutes)));
        }
    }
}