using System.Collections.Generic;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using Microsoft.EntityFrameworkCore;
using TwitchLib;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static BotMain NewBot(TwitchClientSettings twitchSettings, string connectionString)
        {
            var twitchChatClient = new TwitchChatClient(twitchSettings);
            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                twitchChatClient,
            };
            var twitchApi = new TwitchAPI(twitchSettings.TwitchClientId);
            var twitchFollowerService = new TwitchFollowerService(twitchApi, twitchSettings);

            IRepository repository = SetUpDatabase.SetUpRepository(connectionString);


            var currencyGenerator = new CurrencyGenerator(chatClients, repository);

            var simpleCommands = repository.List<SimpleCommand>();

            List<IBotCommand> allCommands = new List<IBotCommand>();
            allCommands.AddRange(simpleCommands);
            allCommands.Add(new CommandsCommand(allCommands));
            allCommands.Add(new CoinsCommand(repository));
            allCommands.Add(new ShoutOutCommand(twitchFollowerService));
            allCommands.Add(new QuoteCommand(repository));
            allCommands.Add(new AddQuoteCommand(repository));
            allCommands.Add(new RockPaperScissorsCommand(currencyGenerator));

            var commandHandler = new CommandHandler(chatClients, allCommands);
            var subscriberHandler = new SubscriberHandler(chatClients);

            var twitchSystem = new FollowableSystem(new[] { twitchChatClient }, twitchFollowerService);

            var currencyUpdate = new CurrencyUpdate(1, currencyGenerator);

            var automatedActionSystem = new AutomatedActionSystem(new List<IIntervalAction> { currencyUpdate });

            var botMain = new BotMain(chatClients, repository, commandHandler, subscriberHandler, twitchSystem, automatedActionSystem);
            return botMain;
        }

    }
}