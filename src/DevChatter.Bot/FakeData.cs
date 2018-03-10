using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Extensions;
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

        private static List<IntervalTriggeredMessage> GetIntervalTriggeredMessages()
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

        private static List<SimpleResponseMessage> GetSimpleResponseMessages()
        {
            return new List<SimpleResponseMessage>
            {
                new SimpleResponseMessage("coins", "Coins?!?! I think you meant !points", DataItemStatus.Active),
                new SimpleResponseMessage("github", "Check out our GitHub repositories here https://github.com/DevChatter/", DataItemStatus.Active),
                new SimpleResponseMessage("emotes", "These are our current emotes: devchaHype devchaDerp devchaFail ", DataItemStatus.Active),
                new SimpleResponseMessage("so", "Hey! We love https://www.twitch.tv/{0} ! You should go check out their channel!", DataItemStatus.Active,
                    x => x.Arguments?.FirstOrDefault()?.NoAt()),
                new SimpleResponseMessage("lurk", "{0} is just lurking here, but still thinks you're all awesome!", DataItemStatus.Active, x => x.ChatUser.DisplayName)
            };
        }

        public void Initialize()
        {
            _repository.Create(GetIntervalTriggeredMessages());

            _repository.Create(GetSimpleResponseMessages());
        }
    }
}