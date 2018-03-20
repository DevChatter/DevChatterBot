using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot
{
    public class FakeData
    {
        private readonly IRepository _repository;

        public FakeData(IRepository repository)
        {
            _repository = repository;
        }

        private static List<IntervalMessage> GetIntervalMessages()
        {
            var automatedMessages = new List<IntervalMessage>
            {
                new IntervalMessage(15,
                    "Hello and welcome! I hope you're enjoying the stream! Feel free to follow along, make suggestions, ask questions, or contribute! And make sure you click the follow button to know when the next stream is!",
                    DataItemStatus.Active),
                new IntervalMessage(1, "foo", DataItemStatus.Draft),
                new IntervalMessage(2, "bar", DataItemStatus.Disabled),
            };
            return automatedMessages;
        }

        private static List<SimpleCommand> GetSimpleCommands()
        {
            return new List<SimpleCommand>
            {
                new SimpleCommand("coins", "Coins?!?! I think you meant !points"),
                new SimpleCommand("discord", "Hey! Checkout out our Discord here https://discord.gg/aQry9jG"),
                new SimpleCommand("github", "Check out our GitHub repositories here https://github.com/DevChatter/"),
                new SimpleCommand("emotes", "These are our current emotes: devchaHype devchaDerp devchaFail "),
                new SimpleCommand("lurk", "[UserDisplayName] is just lurking here, but still thinks you're all awesome!"),
            };
        }

        private List<QuoteEntity> GetInitialQuotes()
        {
            return new List<QuoteEntity>
            {
                new QuoteEntity{QuoteId = 1, DateAdded = new DateTime(2018,3,19),
                    AddedBy = "Brendoneus", Author = "DevChatter", Text = "Hello world!"},
                new QuoteEntity{QuoteId = 2, DateAdded = new DateTime(2018,3,19),
                    AddedBy = "Brendoneus", Author = "DevChatter", Text = "Welcome to DevChatter!"},
            };
        }

        public void Initialize()
        {
            _repository.Create(GetIntervalMessages());

            _repository.Create(GetSimpleCommands());

            _repository.Create(GetInitialQuotes());
        }

    }
}