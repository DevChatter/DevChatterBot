using DevChatter.Bot.Core;
using Xunit;

namespace UnitTests.AutomatedMessagingTests
{
    public class PublishShould
    {
        [Fact]
        public void AddAutomatedMessageToManagedMessages()
        {
            var messagingSystem = new AutomatedMessagingSystem();
            var automatedMessage = new AutomatedMessage
            {
                DelayInMinutes = 1,
                Message = "Welcome! If you are enjoying the content, please follow DevChatter for more!"
            };

            messagingSystem.Publish(automatedMessage);

            Assert.Contains(automatedMessage, messagingSystem.ManagedMessages);
        }
    }
}