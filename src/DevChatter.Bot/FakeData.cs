using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Extensions;
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

        private static List<SimpleCommand> GetSimpleCommands()
        {
            return new List<SimpleCommand>
            {
                new SimpleCommand("coins", "Coins?!?! I think you meant !points"),
                new SimpleCommand("discord", "Hey! Checkout out our Discord here https://discord.gg/aQry9jG"),
                new SimpleCommand("github", "Check out our GitHub repositories here https://github.com/DevChatter/"),
                new SimpleCommand("emotes", "These are our current emotes: devchaHype devchaDerp devchaFail "),
                new SimpleCommand("lurk", "{0} is just lurking here, but still thinks you're all awesome!", selector: x => x.ChatUser.DisplayName)
            };
        }

        public static List<FollowerCommand> GetFollowerCommands()
        {
            return new List<FollowerCommand>
            {
                new FollowerCommand("so", "Hey! We love https://www.twitch.tv/{0} ! You should go check out their channel!", 
                    UserRole.Mod,
                    DataItemStatus.Active,
                    x => x.Arguments?.FirstOrDefault()?.NoAt()),
            };
        }

        public void Initialize()
        {
            _repository.Create(GetIntervalTriggeredMessages());

            _repository.Create(GetSimpleCommands());

            _repository.Create(GetFollowerCommands());
        }
    }
}