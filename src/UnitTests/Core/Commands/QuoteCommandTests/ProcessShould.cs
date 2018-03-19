using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Commands.QuoteCommandTests
{
    public class ProcessShould
    {
        [Fact]
        public void ReturnRequestedQuote_GivenQuoteExists()
        {
            var foundQuote = new QuoteEntity { Message = "Hello world!", Author = "Brendan", QuoteId = 1,
                DateAdded = new DateTime(2018,3,19)};
            var quoteCommand = new QuoteCommand(new FakeRepo() {SingleToReturn = foundQuote});

            var fakeChatClient = new FakeChatClient();
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> { "1" }, CommandWord = "quote"
            };

            quoteCommand.Process(fakeChatClient, commandReceivedEventArgs);

            Assert.Equal($"\"{foundQuote.Message}\" - {foundQuote.Author}, {foundQuote.DateAdded.ToShortDateString()}"
                , fakeChatClient.SentMessage);
        }

        [Fact]
        public void ReturnNotFoundMessage_GivenQuoteNotFound()
        {
            var quoteCommand = new QuoteCommand(new FakeRepo() {SingleToReturn = null});
            int requestQuoteId = 123;

            var fakeChatClient = new FakeChatClient();
            var commandReceivedEventArgs = new CommandReceivedEventArgs
            {
                Arguments = new List<string> { requestQuoteId.ToString() }, CommandWord = "quote"
            };

            quoteCommand.Process(fakeChatClient, commandReceivedEventArgs);

            Assert.Equal($"I'm sorry, but we don't have a quote {requestQuoteId}... Yet...", fakeChatClient.SentMessage);
        }
    }
}