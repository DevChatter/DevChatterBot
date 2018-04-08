using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using TwitchLib;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static BotMain NewBot(BotConfiguration botConfiguration)
        {
            var twitchSettings = botConfiguration.TwitchClientSettings;
            var twitchApi = new TwitchAPI(twitchSettings.TwitchClientId);
            var twitchChatClient = new TwitchChatClient(twitchSettings, twitchApi);
            var chatClients = new List<IChatClient>
            {
                new ConsoleChatClient(),
                twitchChatClient,
            };
            var twitchFollowerService = new TwitchFollowerService(twitchApi, twitchSettings);
            var twitchPlatform = new StreamingPlatform(twitchChatClient, twitchFollowerService, new TwitchStreamingInfoService(twitchApi, twitchSettings));

            IRepository repository = SetUpDatabase.SetUpRepository(botConfiguration.DatabaseConnectionString);

            var chatUserCollection = new ChatUserCollection(repository);
            var currencyGenerator = new CurrencyGenerator(chatClients, chatUserCollection);
            var currencyUpdate = new CurrencyUpdate(1, currencyGenerator);

            var automatedActionSystem = new AutomatedActionSystem(new List<IIntervalAction> { currencyUpdate });
            var rockPaperScissorsGame = new RockPaperScissorsGame(currencyGenerator, automatedActionSystem);
            var wordList = new List<string> { "apple", "banana", "orange", "mango", "watermellon", "grapes", "pizza", "pasta", "pepperoni", "cheese", "mushroom", "csharp", "javascript", "cplusplus", "nullreferenceexception", "parameter", "argument" };
            var hangmanGame = new HangmanGame(currencyGenerator, automatedActionSystem, wordList);

            var simpleCommands = repository.List<SimpleCommand>();
			var aliasCommand = new AliasCommand(repository);

            List<IBotCommand> allCommands = new List<IBotCommand>();
            allCommands.AddRange(simpleCommands);
            allCommands.Add(new UptimeCommand(repository, twitchPlatform));
            allCommands.Add(new GiveCommand(repository, chatUserCollection));
            allCommands.Add(new HelpCommand(repository, allCommands));
            allCommands.Add(new CommandsCommand(repository, allCommands));
            allCommands.Add(new CoinsCommand(repository));
            allCommands.Add(new BonusCommand(repository, currencyGenerator));
            allCommands.Add(new StreamsCommand(repository));
            allCommands.Add(new ShoutOutCommand(repository, twitchFollowerService));
            allCommands.Add(new QuoteCommand(repository));
            allCommands.Add(new AddQuoteCommand(repository));
            allCommands.Add(new AddCommandCommand(repository, allCommands));
            allCommands.Add(new RemoveCommandCommand(repository, allCommands));
            allCommands.Add(new HangmanCommand(repository, hangmanGame));
            allCommands.Add(new RockPaperScissorsCommand(repository, rockPaperScissorsGame));
			allCommands.Add(aliasCommand);

	        foreach (var command in allCommands.OfType<BaseCommand>())
	        {
		        aliasCommand.CommandAliasModified += (s, e) => command.NotifyWordsModified();
	        }

            var commandUsageTracker = new CommandUsageTracker(botConfiguration.CommandHandlerSettings);
            var commandHandler = new CommandHandler(commandUsageTracker, chatClients, allCommands);
            var subscriberHandler = new SubscriberHandler(chatClients);

            var twitchSystem = new FollowableSystem(new[] { twitchChatClient }, twitchFollowerService);


            var botMain = new BotMain(chatClients, repository, commandHandler, subscriberHandler, twitchSystem, automatedActionSystem);
            return botMain;
        }

    }
}