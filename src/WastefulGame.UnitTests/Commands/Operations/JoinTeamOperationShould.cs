using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Commands.Operations;
using DevChatter.Bot.Modules.WastefulGame.Data;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;
using Xunit;

namespace WastefulGame.UnitTests.Commands.Operations
{
    public class JoinTeamOperationShould
    {
        [Fact]
        public void ReturnError_GivenNoTeamSelected()
        {
            var (operation, _, eventArgs) = SetUpTest();

            string result = operation.TryToExecute(eventArgs, new Survivor());

            result.Should().Be(JoinTeamOperation.NO_TEAM_ERROR);
        }

        [Fact]
        public void AddPlayerToTeam_GivenPlayNotOnTeam()
        {
            var (operation, gameRepo, eventArgs) = SetUpTest();
            eventArgs.Arguments.Add("1");
            gameRepo.Setup(x => x.Single(It.IsAny<SurvivorPolicy>()))
                .Returns(new Survivor());

            string result = operation.TryToExecute(eventArgs, new Survivor());

        }

        private (JoinTeamOperation, Mock<IGameRepository>, CommandReceivedEventArgs)
            SetUpTest()
        {
            var gameRepo = new Mock<IGameRepository>();
            var operation = new JoinTeamOperation(gameRepo.Object);
            var eventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> { "join" }
            };
            return (operation, gameRepo, eventArgs);
        }

    }
}
