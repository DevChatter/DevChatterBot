using DevChatter.Bot.Core.BotModules.DuelingModule;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.BotModules.DuelingModule.DuelingSystemTests
{
    public class GetChallengesShould
    {
        [Fact]
        public void ReturnNull_GivenNoDuelsYet()
        {
            var duelingSystem = new DuelingSystem(null);

            Duel challenges = duelingSystem.GetChallenges("Brendan", "Crimson");

            challenges.Should().BeNull();
        }

        [Fact]
        public void ReturnDuel_WhenAlreadyAnOpponent()
        {
            var initialChallenger = "Crimson";
            var acceptingOpponent = "Brendan";

            var duelingSystem = new DuelingSystem(null);

            duelingSystem.RequestDuel(initialChallenger, acceptingOpponent);

            Duel challenges = duelingSystem.GetChallenges(acceptingOpponent, initialChallenger);

            challenges.Should().NotBeNull();
        }

        [Fact]
        public void ReturnNull_WhenTryingToAcceptOwnChallenge()
        {
            var initialChallenger = "Crimson";
            var acceptingOpponent = "Brendan";

            var duelingSystem = new DuelingSystem(null);

            duelingSystem.RequestDuel(initialChallenger, acceptingOpponent);

            Duel challenges = duelingSystem.GetChallenges(initialChallenger, acceptingOpponent);

            challenges.Should().BeNull();
        }
    }
}
