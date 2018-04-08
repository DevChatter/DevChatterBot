using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Commands.QuoteCommandTests
{
    public class ProcessShould
    {
        private QuoteCommand _quoteCommand;
        private readonly FakeChatClient _fakeChatClient = new FakeChatClient();

        private void SetUpTest(QuoteEntity foundQuote, IList<CommandWordEntity> words = null)
        {
            _quoteCommand = new QuoteCommand(new FakeRepo {SingleToReturn = foundQuote});
        }

        [Fact]
        public void ReturnRequestedQuote_GivenQuoteExists()
        {
            QuoteEntity foundQuote = GetTestQuote();
            SetUpTest(foundQuote);

            CommandReceivedEventArgs commandReceivedEventArgs = GetEventArgs(1);

            _quoteCommand.Process(_fakeChatClient, commandReceivedEventArgs);

            Assert.Equal(foundQuote.ToString(), _fakeChatClient.SentMessage);
        }

        [Fact]
        public void ReturnNotFoundMessage_GivenQuoteNotFound()
        {
            SetUpTest(null);
            int requestQuoteId = 123;

            CommandReceivedEventArgs commandReceivedEventArgs = GetEventArgs(requestQuoteId);

            _quoteCommand.Process(_fakeChatClient, commandReceivedEventArgs);

            Assert.Equal($"I'm sorry, but we don't have a quote {requestQuoteId}... Yet...", _fakeChatClient.SentMessage);
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