using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using DevChatter.Bot.Core.Util;
using Moq;
using Xunit;

namespace UnitTests.Core.Automation.AutomationSystemTests
{
    public class AddActionShould
    {
        [Fact]
        public void RunWithoutError()
        {
            var automationSystem = GetTestAutomationSystem();
            var mock = new Mock<IIntervalAction>();

            automationSystem.AddAction(mock.Object);
        }

        [Fact]
        public void ImmediatelyTryToRunTheAction_GivenReadyAction()
        {
            var automationSystem = GetTestAutomationSystem();
            var mock = new Mock<IIntervalAction>();
            mock.Setup(x => x.IsTimeToRun()).Returns(true);

            automationSystem.AddAction(mock.Object);
            automationSystem.ForceRunAllReadyActions();

            mock.Verify(x => x.Invoke(), Times.Once);
        }

        [Fact]
        public void DoNothing_GivenNonReadyAction()
        {
            var automationSystem = GetTestAutomationSystem();
            var mock = new Mock<IIntervalAction>();
            mock.Setup(x => x.IsTimeToRun()).Returns(false);

            automationSystem.AddAction(mock.Object);
            automationSystem.ForceRunAllReadyActions();

            mock.Verify(x => x.Invoke(), Times.Never);
        }

        private static AutomationSystem GetTestAutomationSystem()
        {
            var chatClients = new List<IChatClient>();
            var loggerAdapter = new Mock<ILoggerAdapter<AutomationSystem>>().Object;
            return new AutomationSystem(loggerAdapter, chatClients);
        }
    }
}
