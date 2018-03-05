using System;
using DevChatter.Bot.Core.Messaging;
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

        private static IntervalTriggeredMessage GetTestMessage()
        {
            return new IntervalTriggeredMessage(1, "Hello there!");
        }

        [Fact]
        public void ReturnFalse_AtInitialCreation()
        {
            var intervalTriggeredMessage = GetTestMessage();

            intervalTriggeredMessage.Initialize(_currentTime);

            Assert.False(intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime));
        }

        [Fact]
        public void ReturnTrue_GivenTimeEqualToDelayInMinutes()
        {
            var intervalTriggeredMessage = GetTestMessage();
            intervalTriggeredMessage.Initialize(_currentTime);

            Assert.True(intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime.AddMinutes(1)));
        }

        [Fact]
        public void ReturnFalse_ImmediatelyAfterSendingMessage()
        {
            var intervalTriggeredMessage = GetTestMessage();
            intervalTriggeredMessage.Initialize(_currentTime.AddMinutes(-1));

            intervalTriggeredMessage.GetMessageInstance(_currentTime);

            bool result = intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime);

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrue_OnSecondInterval()
        {
            var intervalTriggeredMessage = GetTestMessage();
            intervalTriggeredMessage.Initialize(_currentTime.AddMinutes(-1));

            intervalTriggeredMessage.GetMessageInstance(_currentTime);

            bool result = intervalTriggeredMessage.IsItYourTimeToDisplay(_currentTime.AddMinutes(1));

            Assert.True(result);
        }
    }
}