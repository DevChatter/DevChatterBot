using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Core.Commands.TopCommandTests
{
    public class GenerateMessageShould
    {
        [Fact]
        public void IncludeNameAndTokens_GivenOnePerson()
        {
            ChatUser chatUser = new ChatUser {DisplayName = "Brendan", Tokens = 10};
            List<ChatUser> topUsers = new List<ChatUser> {chatUser};
            string message = TopCommand.GenerateMessage(topUsers);

            message.Should().Contain(chatUser.DisplayName);
            message.Should().Contain(chatUser.Tokens.ToString());
        }

        [Fact]
        public void IncludeNameAndTokens_GivenTwoPeople()
        {
            ChatUser chatUser1 = new ChatUser {DisplayName = "Brendan", Tokens = 1213};
            ChatUser chatUser2 = new ChatUser {DisplayName = "Pritchett", Tokens = 521};
            List<ChatUser> topUsers = new List<ChatUser> {chatUser1, chatUser2};
            string message = TopCommand.GenerateMessage(topUsers);

            message.Should().Contain(chatUser1.DisplayName);
            message.Should().Contain(chatUser1.Tokens.ToString());
            message.Should().Contain(chatUser2.DisplayName);
            message.Should().Contain(chatUser2.Tokens.ToString());
        }

        [Fact]
        public void NotThrow_GivenNoUsers()
        {
            List<ChatUser> topUsers = new List<ChatUser>();
            string message = TopCommand.GenerateMessage(topUsers);
        }
    }
}
