using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using TwitchLib;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static BotMain NewBot(TwitchClientSettings twitchSettings, IRepository repository)
        {
            var twitchChatClient = new TwitchChatClient(twitchSettings);
            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                twitchChatClient,
            };
            var twitchApi = new TwitchAPI(twitchSettings.TwitchClientId);
            var twitchFollowerService = new TwitchFollowerService(twitchApi, twitchSettings);

            var simpleResponses = repository.List(DataItemPolicy<SimpleCommand>.ActiveOnly());
            var followerCommands = repository.List(DataItemPolicy<FollowsCommand>.ActiveOnly());
            foreach (FollowsCommand followerCommand in followerCommands)
            {
                followerCommand.Initialize(twitchFollowerService);
            }

            List<IBotCommand> allCommands = new List<IBotCommand>();
            allCommands.AddRange(simpleResponses);
            allCommands.AddRange(followerCommands);

            var commandHandler = new CommandHandler(chatClients, allCommands);
            var subscriberHandler = new SubscriberHandler(chatClients);

            var twitchSystem = new FollowableSystem(new [] { twitchChatClient }, twitchFollowerService);

            var botMain = new BotMain(chatClients, repository, commandHandler, subscriberHandler, twitchSystem);
            return botMain;
        }
    }
}