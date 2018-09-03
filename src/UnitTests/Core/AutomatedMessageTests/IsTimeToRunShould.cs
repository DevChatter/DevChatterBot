using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.AutomatedMessageTests
{
    public class IsTimeToRunShould
    {
        private static int _delayInMinutes = 1;

        [Fact]
        public void ReturnFalse_AtInitialCreation()
        {
            (AutomatedMessage message, _) = GetTestMessage();

            Assert.False(message.IsTimeToRun());
        }

        [Fact]
        public void ReturnTrue_GivenTimeEqualToDelayInMinutes()
        {
            (AutomatedMessage message, FakeClock clock) = GetTestMessage();

            clock.UtcNow = clock.UtcNow.AddMinutes(_delayInMinutes).AddSeconds(1); // wait a minute

            Assert.True(message.IsTimeToRun());
        }

        [Fact]
        public void ReturnFalse_ImmediatelyAfterSendingMessage()
        {
            (AutomatedMessage message, _) = GetTestMessage();

            message.Invoke(); // Will reset the interval time

            Assert.False(message.IsTimeToRun());
        }

        [Fact]
        public void ReturnTrue_OnSecondInterval()
        {
            (AutomatedMessage message, FakeClock clock) = GetTestMessage();

            message.Invoke(); // Will reset the interval time

            clock.UtcNow = clock.UtcNow.AddMinutes(_delayInMinutes); // Wait a minute

            Assert.True(message.IsTimeToRun());
        }

        private static (AutomatedMessage, FakeClock) GetTestMessage()
        {
            var fakeClock = new FakeClock();
            var intervalMessage = new AutomatedMessage(new IntervalMessage(_delayInMinutes, "Hello there!"), new List<IChatClient>(), new Mock<IRepository>().Object, fakeClock);
            return (intervalMessage, fakeClock);
        }
    }
}
