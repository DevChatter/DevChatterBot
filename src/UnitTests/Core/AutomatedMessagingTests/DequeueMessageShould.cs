using System;
using DevChatter.Bot.Core.Messaging;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.AutomatedMessagingTests
{
    public class DequeueMessageShould
    {
        [Fact]
        public void ReturnFalse_GivenEmptyQueue()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();

            bool result = automatedMessagingSystem.DequeueMessage(out string myMessage);

            Assert.False(result);
        }

        [Fact]
        public void ReturnTrueAndProvideMessage_GivenSingleQueuedMessage()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();
            var message = new AlwaysReadyMessage();
            automatedMessagingSystem.Publish(message);
            automatedMessagingSystem.CheckMessages(DateTime.Now);

            bool result = automatedMessagingSystem.DequeueMessage(out string myMessage);

            Assert.True(result);
            Assert.Equal(message.GetMessageInstance(DateTime.Now), myMessage);
        }

        [Fact]
        public void ReturnOnlyOneMessage_GivenSingleQueuedMessage()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();
            var message = new AlwaysReadyMessage();
            automatedMessagingSystem.Publish(message);
            automatedMessagingSystem.CheckMessages(DateTime.Now);

            automatedMessagingSystem.DequeueMessage(out string myMessage);
            bool result = automatedMessagingSystem.DequeueMessage(out string throwawayMessage);

            Assert.False(result);
            Assert.Equal(message.GetMessageInstance(DateTime.Now), myMessage);
            Assert.Null(throwawayMessage);
        }
    }
}