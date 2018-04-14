using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.Core.Commands.TopBallerCommandTests
{
    public class ProcessShould
    {
        private TopBallersCommand _topBallersCommand;
        private readonly Mock<IChatClient> _chatClientMock = new Mock<IChatClient>();
        private readonly Mock<IRepository> _repositoryMock = new Mock<IRepository>();

        private void SetUpTest(List<ChatUser> ballers)
        {
            _repositoryMock.Setup(x => x.List(It.IsAny<ISpecification<ChatUser>>())).Returns(ballers);
            _topBallersCommand = new TopBallersCommand(_repositoryMock.Object);
        }

        [Fact]
        public void ReturnFiveBallers_GivenTestData()
        {
            List<ChatUser> testBallers = GetTestUser();
            SetUpTest(testBallers);
            List<ChatUser> actualBallers = _topBallersCommand.TopFiveBallers();
            Assert.Equal(5, actualBallers.Count);
        }

        [Fact]
        public void ReturnChatUserwithHighestTokens_GivenTestData()
        {
            List<ChatUser> testBallers = GetTestUser();
            SetUpTest(testBallers);
            List<ChatUser> actualBallers = _topBallersCommand.TopFiveBallers();
            Assert.Equal(testBallers.Max(b => b.Tokens), actualBallers[0].Tokens);
        }

        // Test Data has 6 users
        // This test ensure the sixth user (lowest tokens) is not in the list
        [Fact]
        public void EnsureSixthHighestTokenBallerIsNotIncludedInList_GivenTestData()
        {
            List<ChatUser> testBallers = GetTestUser();
            SetUpTest(testBallers);
            List<ChatUser> actualBallers = _topBallersCommand.TopFiveBallers();
            Assert.NotEqual(testBallers.Min(b => b.Tokens),  actualBallers.Min(b => b.Tokens));
        }

        [Fact]
        public void EnsureMessageIsCorrect_GivenTestData()
        {
            List<ChatUser> testBallers = GetTestUser();
            SetUpTest(testBallers);
            List<ChatUser> actualBallers = _topBallersCommand.TopFiveBallers();
            string testMessage = TestGenerateMessage(actualBallers);
            string actualMessage = _topBallersCommand.GenerateMessage(actualBallers);
            Assert.Equal(testMessage, actualMessage);
        }

        private string TestGenerateMessage(List<ChatUser> ballers)
        {
            string message = $"This channel's Top Ballers are: ";
            for (int i = 0; i < ballers.Count; i++)
            {
                message += $"-|-   {i+1}. {ballers[i].DisplayName}:{ballers[i].Tokens}   ";
            }

            return message;
        }
        private static CommandReceivedEventArgs GetEventArgs(int requestQuoteId)
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> { requestQuoteId.ToString() },
                CommandWord = "quote"
            };
            return commandReceivedEventArgs;
        }

        private static List<ChatUser> GetTestUser()
        {
            List<ChatUser> ballers = new List<ChatUser> {
                new ChatUser()
            {
                DisplayName = "Kaladin",
                Tokens = 40
            },
            new ChatUser()
            {
                DisplayName = "Shallan",
                Tokens = 39
            },
            new ChatUser()
            {
                DisplayName = "Dalinar",
                Tokens = 38
            },
            new ChatUser()
            {
                DisplayName = "Jasnah",
                Tokens = 37
            },
            new ChatUser()
            {
                DisplayName = "Szeth",
                Tokens = 36
            },
            new ChatUser()
            {
                DisplayName = "Rock",
                Tokens = 35
            }
            };

            return ballers;
        }
    }
}
