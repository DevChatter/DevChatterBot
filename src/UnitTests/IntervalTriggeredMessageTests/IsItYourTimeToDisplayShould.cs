using System;
using DevChatter.Bot.Core;
using Xunit;

namespace UnitTests.IntervalTriggeredMessageTests
{
    public class IsItYourTimeToDisplayShould
    {
        private readonly DateTime _currentTime;

        public IsItYourTimeToDisplayShould()
        {
            _currentTime = DateTime.Now;
        }

        [Fact]
        public void ReturnFalse_AtInitialCreation()
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage { DelayInMinutes = 1, Message = "Hello there!" };
            intervalTriggeredMessage.Initialize(_currentTime);

            Assert.False(intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime));
        }

        [Fact]
        public void ReturnTrue_GivenTimeEqualToDelayInMinutes()
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage { DelayInMinutes = 1, Message = "Hello there!" };
            intervalTriggeredMessage.Initialize(_currentTime);

            Assert.True(intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime.AddMinutes(intervalTriggeredMessage.DelayInMinutes)));
        }

        [Fact]
        public void ReturnFalse_ImmediatelyAfterSendingMessage()
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage { DelayInMinutes = 1, Message = "Hello there!" };
            intervalTriggeredMessage.Initialize(_currentTime.AddMinutes(-1));

            intervalTriggeredMessage.GetMessageInstance(_currentTime);

            bool result = intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime);

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrue_OnSecondInterval()
        {
            var intervalTriggeredMessage = new IntervalTriggeredMessage { DelayInMinutes = 1, Message = "Hello there!" };
            intervalTriggeredMessage.Initialize(_currentTime.AddMinutes(-1));

            intervalTriggeredMessage.GetMessageInstance(_currentTime);

            bool result = intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime.AddMinutes(1));

            Assert.True(result);
        }
    }
}