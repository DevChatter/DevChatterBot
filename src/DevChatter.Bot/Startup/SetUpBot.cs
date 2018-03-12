using System;
using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using TwitchLib;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static BotMain NewBot(TwitchClientSettings twitchSettings, IRepository repository)
        {
            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                new TwitchChatClient(twitchSettings),
            };
            var twitchApi = new TwitchAPI(twitchSettings.TwitchClientId);
            var twitchFollowerService = new TwitchFollowerService(twitchApi, twitchSettings);

            var simpleResponses = repository.List(DataItemPolicy<SimpleResponseMessage>.ActiveOnly());
            var followerCommands = repository.List(DataItemPolicy<FollowerCommand>.ActiveOnly());
            foreach (FollowerCommand followerCommand in followerCommands)
            {
                followerCommand.Initialize(twitchFollowerService);
            }

            List<ICommandMessage> allCommands = new List<ICommandMessage>();
            allCommands.AddRange(simpleResponses);
            allCommands.AddRange(followerCommands);

            var commandHandler = new CommandHandler(chatClients, allCommands);
            var subscriberHandler = new SubscriberHandler(chatClients);
            var botMain = new BotMain(chatClients, repository, commandHandler, subscriberHandler, twitchFollowerService);
            return botMain;
        }
    }
}