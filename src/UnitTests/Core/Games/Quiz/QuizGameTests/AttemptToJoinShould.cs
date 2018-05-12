using System;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Systems.Chat;
using FluentAssertions;
using Moq;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Games.Quiz.QuizGameTests
{
    public class AttemptToJoinShould
    {
        [Fact]
        public void PreventJoining_GivenAlreadyCompetingUser()
        {
            var quizGame = new QuizGame(new Mock<ICurrencyGenerator>().Object, new Mock<IAutomatedActionSystem>().Object);

            var chatUser = new ChatUser {DisplayName = "Brendan"};
            quizGame.StartGame(new Mock<IChatClient>().Object);
            quizGame.AttemptToJoin(chatUser);
            var result = quizGame.AttemptToJoin(chatUser);

            result.Should().Be(QuizJoinResults.AlreadyInGameResult(chatUser.DisplayName));
        }

        [Fact]
        public void PreventJoining_GivenGameNotStarted()
        {
            var quizGame = new QuizGame(new Mock<ICurrencyGenerator>().Object, new Mock<IAutomatedActionSystem>().Object);

            string displayName = Guid.NewGuid().ToString();
            var result = quizGame.AttemptToJoin(new ChatUser {DisplayName = displayName});

            result.Should().Be(QuizJoinResults.NotJoinTimeResult(displayName));
        }

        [Fact]
        public void PreventJoining_GivenQuestionsBeingAsked()
        {
            var automatedActionSystem = new FakeActionSystem();
            var quizGame = new QuizGame(new Mock<ICurrencyGenerator>().Object, automatedActionSystem);
            string displayName = Guid.NewGuid().ToString();
            quizGame.StartGame(new Mock<IChatClient>().Object);
            automatedActionSystem.IntervalAction.Invoke(); // run the action, starting the questions

            var result = quizGame.AttemptToJoin(new ChatUser {DisplayName = displayName});

            result.Should().Be(QuizJoinResults.NotJoinTimeResult(displayName));
        }

        [Fact]
        public void AllowJoining_GivenNewUserDuringJoinWindow()
        {
            var quizGame = new QuizGame(new Mock<ICurrencyGenerator>().Object, new Mock<IAutomatedActionSystem>().Object);

            string displayName = Guid.NewGuid().ToString();
            quizGame.StartGame(new Mock<IChatClient>().Object);
            var result = quizGame.AttemptToJoin(new ChatUser {DisplayName = displayName});

            result.Should().Be(QuizJoinResults.SuccessJoinResult(displayName));
        }
    }
}
