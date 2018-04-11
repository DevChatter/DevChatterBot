using System;
using DevChatter.Bot.Core.Messaging;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.IntervalMessageTests
{
    public class IsTimeToDisplayShould
    {
        private static int _delayInMinutes = 1;

        [Fact]
        public void ReturnFalse_AtInitialCreation()
        {
            (IntervalMessage message, _) = GetTestMessage();

            Assert.False(message.IsTimeToDisplay());
        }

        [Fact]
        public void ReturnTrue_GivenTimeEqualToDelayInMinutes()
        {
            (IntervalMessage message, FakeClock clock) = GetTestMessage();

            clock.Now = DateTime.Now.AddMinutes(_delayInMinutes); // wait a minute

            Assert.True(message.IsTimeToDisplay());
        }

        [Fact]
        public void ReturnFalse_ImmediatelyAfterSendingMessage()
        {
            (IntervalMessage message, _) = GetTestMessage();

            message.GetMessageInstance(); // Will reset the interval time

            Assert.False(message.IsTimeToDisplay());
        }

        [Fact]
        public void ReturnTrue_OnSecondInterval()
        {
            (IntervalMessage message, FakeClock clock) = GetTestMessage();

            message.GetMessageInstance(); // Will reset the interval time

            clock.Now = clock.Now.AddMinutes(_delayInMinutes); // Wait a minute

            Assert.True(message.IsTimeToDisplay());
        }

        private static (IntervalMessage, FakeClock) GetTestMessage()
        {
            var fakeClock = new FakeClock();
            var intervalMessage = new IntervalMessage(_delayInMinutes, "Hello there!", fakeClock);
            return (intervalMessage, fakeClock);
        }
    }
}
