using System;
using DevChatter.Bot.Core.Messaging;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.AutomatedMessagingTests
{
    public class CheckMessagesShould
    {
        [Fact]
        public void AddNoMessagesToQueue_GivenNoAutomatedMessages()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();

            automatedMessagingSystem.CheckMessages();

            Assert.Empty(automatedMessagingSystem.QueuedMessages);
        }

        [Fact]
        public void AddOneMessagesToQueue_GivenOneReadyAutomatedMessage()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();
            automatedMessagingSystem.Publish(new AlwaysReadyMessage());

            automatedMessagingSystem.CheckMessages();

            Assert.Single(automatedMessagingSystem.QueuedMessages);
        }

        [Fact]
        public void AddOneMessagesToQueue_GivenManyMessages_OnlyOneReady()
        {
            var automatedMessagingSystem = new AutomatedMessagingSystem();
            automatedMessagingSystem.Publish(new NeverReadyMessage());
            automatedMessagingSystem.Publish(new AlwaysReadyMessage());
            automatedMessagingSystem.Publish(new NeverReadyMessage());

            automatedMessagingSystem.CheckMessages();

            Assert.Single(automatedMessagingSystem.QueuedMessages);
        }
    }
}
