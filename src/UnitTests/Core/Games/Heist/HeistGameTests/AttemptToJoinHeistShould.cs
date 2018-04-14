using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Games.Heist;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Games.Heist.HeistGameTests
{
    public class AttemptToJoinHeistShould
    {
        [Fact]
        public void AllowJoining_GivenRandomSelectionForFirstTimer()
        {
            var heistGame = new HeistGame(new Mock<IAutomatedActionSystem>().Object);

            var displayName = "nameOfPerson";
            JoinGameResult attemptToJoinHeist = heistGame.AttemptToJoinHeist(displayName, out HeistRoles role);

            attemptToJoinHeist.Should().Be(HeistJoinResults.SuccessJoinResult(displayName, role));
        }

        [Fact]
        public void DisallowJoining_GivenSameJoiner()
        {
            var heistGame = new HeistGame(new Mock<IAutomatedActionSystem>().Object);

            var displayName = "nameOfPerson";
            JoinGameResult attemptToJoinHeist1 = heistGame.AttemptToJoinHeist(displayName, out HeistRoles role1);
            JoinGameResult attemptToJoinHeist2 = heistGame.AttemptToJoinHeist(displayName, out HeistRoles role2);

            attemptToJoinHeist1.Should().Be(HeistJoinResults.SuccessJoinResult(displayName, role1));
            attemptToJoinHeist2.Should().Be(HeistJoinResults.AlreadyInHeistResult(displayName));
        }
    }
}
