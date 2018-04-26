using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Core.Commands.TopCommandTests
{
    public class ProcessShould
    {
        private TopCommand _topCommand;
        private readonly Mock<IChatClient> _chatClientMock = new Mock<IChatClient>();
        private readonly Mock<IRepository> _repositoryMock = new Mock<IRepository>();

        private void SetUpTest(List<ChatUser> users)
        {
            _repositoryMock.Setup(x => x.List(It.IsAny<ISpecification<ChatUser>>())).Returns(users);
            _topCommand = new TopCommand(_repositoryMock.Object);
        }

        [Fact]
        public void ReturnFiveUsers_GivenTestData()
        {
            List<ChatUser> testBallers = GetTestUsers();
            SetUpTest(testBallers);
            List<ChatUser> actualBallers = _topCommand.GetTopUsers();
            Assert.Equal(5, actualBallers.Count);
        }

        [Fact]
        public void ReturnChatUserWithHighestTokens_GivenTestData()
        {
            List<ChatUser> testBallers = GetTestUsers();
            SetUpTest(testBallers);
            List<ChatUser> actualBallers = _topCommand.GetTopUsers();
            Assert.Equal(testBallers.Max(b => b.Tokens), actualBallers[0].Tokens);
        }

        [Fact]
        public void EnsureSixthHighestUserIsNotIncludedInList_GivenTestData()
        {
            List<ChatUser> users = GetTestUsers();
            SetUpTest(users);
            List<ChatUser> topUsers = _topCommand.GetTopUsers();
            Assert.DoesNotContain(topUsers, u => u.DisplayName == "Rock");
        }

        [Fact]
        public void EnsureMessageDoesNotContainSixthHighestUsers_GivenTestData()
        {
            List<ChatUser> users = GetTestUsers();
            SetUpTest(users);
            List<ChatUser> topUsers = _topCommand.GetTopUsers();
            string message = TopCommand.GenerateMessage(topUsers);
            Assert.DoesNotContain("Rock", message);
        }

        [Fact]
        public void DisplayTopUsers_GivenTestData()
        {
            List<ChatUser> users = GetTestUsers();
            SetUpTest(users);
            _topCommand.DisplayTopUsers(_chatClientMock.Object);
            List<ChatUser> topUsers = _topCommand.GetTopUsers();
            string message = TopCommand.GenerateMessage(topUsers);
            Assert.DoesNotContain("Rock", message);
        }

        [Fact]
        public void ReturnCorrectMessageFromProcessMethod_GivenTestData()
        {
            List<ChatUser> users = GetTestUsers();
            SetUpTest(users);
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = null,
                CommandWord = "top"
            };
            _topCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);
            List<ChatUser> topUsers = _topCommand.GetTopUsers();
            string message = TopCommand.GenerateMessage(topUsers);
            _chatClientMock.Verify(x => x.SendMessage(message));
        }

        private static List<ChatUser> GetTestUsers()
        {
            List<ChatUser> users = new List<ChatUser> {
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

            return users;
        }
    }
}
