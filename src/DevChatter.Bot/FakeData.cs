using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot
{
    public class FakeData
    {
        private readonly IRepository _repository;

        public FakeData(IRepository repository)
        {
            _repository = repository;
        }

        private static List<IntervalTriggeredMessage> GetIAutomatedMessage()
        {
            var automatedMessages = new List<IntervalTriggeredMessage>
            {
                new IntervalTriggeredMessage(15,
                    "Hello and welcome! I hope you're enjoying the stream! Feel free to follow along, make suggestions, ask questions, or contribute! And make sure you click the follow button to know when the next stream is!",
                    DataItemStatus.Active),
                new IntervalTriggeredMessage(1, "foo", DataItemStatus.Draft),
                new IntervalTriggeredMessage(2, "bar", DataItemStatus.Disabled),
            };
            return automatedMessages;
        }

        private static List<SimpleResponseMessage> GetICommandMessages()
        {
            return new List<SimpleResponseMessage>
            {
                new SimpleResponseMessage("coins", "Coins?!?! I think you meant !points", DataItemStatus.Active),
            };
        }

        public void Initialize()
        {
            _repository.Create(GetIAutomatedMessage());

            _repository.Create(GetICommandMessages());
        }
    }
}