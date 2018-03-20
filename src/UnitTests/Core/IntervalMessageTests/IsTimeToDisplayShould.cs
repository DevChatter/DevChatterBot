using System;
using DevChatter.Bot.Core.Messaging;
using Pose;
using Xunit;

namespace UnitTests.Core.IntervalMessageTests
{
    public class IsTimeToDisplayShould
    {
        [Fact]
        public void ReturnFalse_AtInitialCreation()
        {
            var message = GetTestMessage();

            Assert.False(message.IsTimeToDisplay());
        }

        [Fact]
        public void ReturnTrue_GivenTimeEqualToDelayInMinutes()
        {
            Shim shim = Shim.Replace(() => DateTime.Now).With(() => DateTime.Now.AddMinutes(1));
            var message = GetTestMessage();

            PoseContext.Isolate(() => Assert.True(message.IsTimeToDisplay()), shim);
        }

        [Fact]
        public void ReturnFalse_ImmediatelyAfterSendingMessage()
        {
            var message = GetTestMessage();

            message.GetMessageInstance(); // Will reset the interval time
            Assert.False(message.IsTimeToDisplay());
        }

        [Fact]
        public void ReturnTrue_OnSecondInterval()
        {
            Shim shim = Shim.Replace(() => DateTime.Now).With(() => DateTime.Now.AddMinutes(1));
            var message = GetTestMessage();

            message.GetMessageInstance(); // Will reset the interval time
            PoseContext.Isolate(() => Assert.True(message.IsTimeToDisplay()), shim);
        }

        private static IntervalMessage GetTestMessage()
        {
            return new IntervalMessage(1, "Hello there!");
        }
    }
}