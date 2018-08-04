using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Systems.Chat;
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

            clock.UtcNow = clock.UtcNow.AddMinutes(_delayInMinutes); // wait a minute

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
            var intervalMessage = new AutomatedMessage("Hello there!", _delayInMinutes,
                new List<IChatClient>(), fakeClock);
            return (intervalMessage, fakeClock);
        }
    }
}
