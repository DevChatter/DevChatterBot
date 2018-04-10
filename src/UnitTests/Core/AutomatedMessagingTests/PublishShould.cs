using DevChatter.Bot.Core.Messaging;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.AutomatedMessagingTests
{
    public class PublishShould
    {
        [Fact]
        public void AddAutomatedMessageToManagedMessages()
        {
            var messagingSystem = new AutomatedMessagingSystem();
            var automatedMessage = new IntervalMessage(1, "Welcome! If you are enjoying the content, please follow DevChatter for more!", new FakeClock());

            messagingSystem.Publish(automatedMessage);

            Assert.Contains(automatedMessage, messagingSystem.ManagedMessages);
        }
    }
}