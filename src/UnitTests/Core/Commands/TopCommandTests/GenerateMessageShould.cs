using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Core.Commands.TopCommandTests
{
    public class GenerateMessageShould
    {
        [Fact]
        public void IncludeNameAndTokens_GivenOnePerson()
        {
            var command = new TopCommand(new Mock<IRepository>().Object);

            ChatUser chatUser = new ChatUser { DisplayName = "Brendan", Tokens = 10 };
            List<ChatUser> topUsers = new List<ChatUser> { chatUser };
            string message = command.GenerateMessage(topUsers);

            message.Should().Contain(chatUser.DisplayName);
            message.Should().Contain(chatUser.Tokens.ToString());
        }

        [Fact]
        public void IncludeNameAndTokens_GivenTwoPeople()
        {
            var command = new TopCommand(new Mock<IRepository>().Object);

            ChatUser chatUser1 = new ChatUser { DisplayName = "Brendan", Tokens = 1213 };
            ChatUser chatUser2 = new ChatUser { DisplayName = "Pritchett", Tokens = 521 };
            List<ChatUser> topUsers = new List<ChatUser> { chatUser1, chatUser2 };
            string message = command.GenerateMessage(topUsers);

            message.Should().Contain(chatUser1.DisplayName);
            message.Should().Contain(chatUser1.Tokens.ToString());
            message.Should().Contain(chatUser2.DisplayName);
            message.Should().Contain(chatUser2.Tokens.ToString());
        }

        [Fact]
        public void NotThrow_GivenNoUsers()
        {
            var command = new TopCommand(new Mock<IRepository>().Object);

            List<ChatUser> topUsers = new List<ChatUser>();
            string message = command.GenerateMessage(topUsers);
        }
    }
}
