using System.Collections.Generic;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using FluentAssertions;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Automation.AutomationSystemTests
{
    public class AutomationSystemShould
    {
        [Fact]
        public void RemoveActions_GivenTheyWillNeverRunAgain()
        {
            var fakeToKeep = new FakeIntervalAction(true, false);
            var fakeToRemove = new FakeIntervalAction(true, true);
            var automationSystem = GetTestAutomationSystem();

            automationSystem.AddAction(fakeToKeep);
            automationSystem.AddAction(fakeToRemove);

            automationSystem.ForceRemoveActionsThatWillNeverRunAgain();
            automationSystem.ForceRunAllReadyActions();

            fakeToKeep.InvokeWasRun.Should().BeTrue();
            fakeToRemove.InvokeWasRun.Should().BeFalse();
        }

        private static AutomationSystem GetTestAutomationSystem()
        {
            var chatClients = new List<IChatClient>();
            var loggerAdapter = new Mock<ILoggerAdapter<AutomationSystem>>().Object;
            return new AutomationSystem(loggerAdapter, chatClients);
        }

    }
}
