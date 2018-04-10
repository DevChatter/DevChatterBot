using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using Moq;
using Xunit;

namespace UnitTests.Core.Commands.QuoteCommandTests
{
    public class ProcessShould
    {
        private QuoteCommand _quoteCommand;
        private readonly Mock<IChatClient> _chatClientMock = new Mock<IChatClient>();
        private readonly Mock<IRepository> _repositoryMock = new Mock<IRepository>();

        private void SetUpTest(QuoteEntity foundQuote)
        {
            _repositoryMock.Setup(x => x.Single(It.IsAny<ISpecification<QuoteEntity>>())).Returns(foundQuote);
            _quoteCommand = new QuoteCommand(_repositoryMock.Object);
        }

        [Fact]
        public void ReturnRequestedQuote_GivenQuoteExists()
        {
            QuoteEntity foundQuote = GetTestQuote();
            SetUpTest(foundQuote);

            CommandReceivedEventArgs commandReceivedEventArgs = GetEventArgs(1);

            _quoteCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);

            _chatClientMock.Verify(x => x.SendMessage(foundQuote.ToString()));
        }

        [Fact]
        public void ReturnNotFoundMessage_GivenQuoteNotFound()
        {
            SetUpTest(null);
            int requestQuoteId = 123;

            CommandReceivedEventArgs commandReceivedEventArgs = GetEventArgs(requestQuoteId);

            _quoteCommand.Process(_chatClientMock.Object, commandReceivedEventArgs);

            _chatClientMock.Verify(x => x.SendMessage($"I'm sorry, but we don't have a quote {requestQuoteId}... Yet..."));
        }

        private static CommandReceivedEventArgs GetEventArgs(int requestQuoteId)
        {
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> {requestQuoteId.ToString()},
                CommandWord = "quote"
            };
            return commandReceivedEventArgs;
        }

        private static QuoteEntity GetTestQuote()
        {
            var foundQuote = new QuoteEntity
            {
                Text = "Hello world!",
                Author = "Brendan",
                QuoteId = 1,
                DateAdded = new DateTime(2018, 3, 19)
            };
            return foundQuote;
        }


    }
}