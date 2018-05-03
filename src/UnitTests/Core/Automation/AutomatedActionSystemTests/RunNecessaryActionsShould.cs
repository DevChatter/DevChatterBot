using DevChatter.Bot.Core.Automation;
using Moq;
using System.Collections.Generic;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Automation.AutomatedActionSystemTests
{
    public class RunNecessaryActionsShould
    {
        [Fact]
        public void InvokeAction_GivenOneReadyAction()
        {
            var mock = new Mock<IIntervalAction>();
            AutomatedActionSystem automateActionSystem = SetUpTestActionSystem(mock.Object);

            mock.Setup(x => x.IsTimeToRun()).Returns(true);

            automateActionSystem.RunNecessaryActions();

            mock.Verify(x => x.Invoke(), Times.Once());
        }

        [Fact]
        public void InvokeActions_GivenReadyActions()
        {
            var mock = new Mock<IIntervalAction>();
            AutomatedActionSystem automateActionSystem = SetUpTestActionSystem(mock.Object, mock.Object);

            mock.Setup(x => x.IsTimeToRun()).Returns(true);

            automateActionSystem.RunNecessaryActions();

            mock.Verify(x => x.Invoke(), Times.Exactly(2));
        }

        [Fact]
        public void InvokeNoActions_GivenNoActions()
        {
            AutomatedActionSystem automateActionSystem = SetUpTestActionSystem(new IIntervalAction[0]);

            automateActionSystem.RunNecessaryActions();
        }

        [Fact]
        public void InvokeNoActions_GivenNoReadyActions()
        {
            var mock = new Mock<IIntervalAction>();
            AutomatedActionSystem automateActionSystem = SetUpTestActionSystem(mock.Object, mock.Object);

            mock.Setup(x => x.IsTimeToRun()).Returns(false);

            automateActionSystem.RunNecessaryActions();

            mock.Verify(x => x.Invoke(), Times.Never());
        }

        [Fact]
        public void InvokeOnlyReadyActions_GivenMultipleStatesOfActions()
        {
            var mock1 = new Mock<IIntervalAction>();
            var mock2 = new Mock<IIntervalAction>();
            AutomatedActionSystem automateActionSystem = SetUpTestActionSystem(mock1.Object, mock2.Object);

            mock1.Setup(x => x.IsTimeToRun()).Returns(true);
            mock2.Setup(x => x.IsTimeToRun()).Returns(false);

            automateActionSystem.RunNecessaryActions();

            mock1.Verify(x => x.Invoke(), Times.Once());
            mock2.Verify(x => x.Invoke(), Times.Never());
        }

        private static AutomatedActionSystem SetUpTestActionSystem(params IIntervalAction[] actions)
        {
            var intervalActions = new List<IIntervalAction>(actions);
            var automateActionSystem = new AutomatedActionSystem(intervalActions);
            return automateActionSystem;
        }
    }
}
