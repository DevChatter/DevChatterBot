using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Infra.Twitch;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static BotMain NewBot(TwitchClientSettings clientSettings, IRepository repository)
        {
            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                new TwitchChatClient(clientSettings),
            };

            var commandMessages = repository.List(DataItemPolicy<SimpleResponseMessage>.ActiveOnly());
            var commandHandler = new CommandHandler(chatClients, commandMessages);
            var subscriberHandler = new SubscriberHandler(chatClients);
            var followerHandler = new FollowerHandler(chatClients);
            var botMain = new BotMain(chatClients, repository, commandHandler, subscriberHandler, followerHandler);
            return botMain;
        }
    }
}