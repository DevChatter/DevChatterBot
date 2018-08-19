using System;
using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.Core.Events.CurrencyGeneratorTests
{
    public class AddCurrencyToShould
    {
        [Fact]
        public void AllowGoingToMaxInt_GivenZeroStartAndMaxIntAdded()
        {
            var mockRepo = new Mock<IRepository>();
            var currencyGenerator = new CurrencyGenerator(new List<IChatClient>(), new ChatUserCollection(mockRepo.Object), new Mock<ISettingsFactory>().Object);

            var chatUser = new ChatUser {Tokens = 0};
            mockRepo.Setup(x => x.List(It.IsAny<ChatUserPolicy>())).Returns(new List<ChatUser> { chatUser });

            currencyGenerator.AddCurrencyTo("twitchloff", int.MaxValue);

            chatUser.Tokens.Should().Be(int.MaxValue);
        }

        [Fact]
        public void CapAtMaxInt_GivenOverflowPossibility()
        {
            var mockRepo = new Mock<IRepository>();
            var currencyGenerator = new CurrencyGenerator(new List<IChatClient>(), new ChatUserCollection(mockRepo.Object), new Mock<ISettingsFactory>().Object);

            var chatUser = new ChatUser {Tokens = 100};
            mockRepo.Setup(x => x.List(It.IsAny<ChatUserPolicy>())).Returns(new List<ChatUser> { chatUser });

            Action testCode = () => currencyGenerator.AddCurrencyTo("twitchloff", int.MaxValue);
            testCode.Should().Throw<OverflowException>();
        }
    }
}
