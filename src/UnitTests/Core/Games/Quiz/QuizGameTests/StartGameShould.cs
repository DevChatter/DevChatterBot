using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Systems.Chat;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Games.Quiz.QuizGameTests
{
    public class IsRunningShould
    {
        [Fact]
        public void BeFalse_ByDefault()
        {
            var quizGame = new QuizGame(new Mock<ICurrencyGenerator>().Object, new Mock<IAutomatedActionSystem>().Object);

            quizGame.IsRunning.Should().BeFalse();
        }

        [Fact]
        public void BeTrue_AfterStarting()
        {
            var quizGame = new QuizGame(new Mock<ICurrencyGenerator>().Object, new Mock<IAutomatedActionSystem>().Object);

            quizGame.StartGame(new Mock<IChatClient>().Object);

            quizGame.IsRunning.Should().BeTrue();
        }

        // TODO: Add the test to confirm that the game is no longer running after it's completed.
    }
}
