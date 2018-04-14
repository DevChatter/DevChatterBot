using Autofac;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Util;
using TwitchLib;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static IContainer NewBotDepedencyContainer(BotConfiguration botConfiguration)
        {
            var repository = SetUpDatabase.SetUpRepository(botConfiguration.DatabaseConnectionString);

            var builder = new ContainerBuilder();

            builder.Register(ctx => botConfiguration.CommandHandlerSettings).AsSelf().SingleInstance();
            builder.Register(ctx => botConfiguration.TwitchClientSettings).AsSelf().SingleInstance();

            builder.RegisterType<TwitchFollowerService>().AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx => new TwitchAPI(botConfiguration.TwitchClientSettings.TwitchClientId))
                .AsImplementedInterfaces();
            builder.RegisterType<TwitchChatClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StreamingPlatform>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TwitchStreamingInfoService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SystemClock>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ConsoleChatClient>().AsImplementedInterfaces().SingleInstance();

            builder.Register(ctx => repository).As<IRepository>().SingleInstance();

            builder.RegisterType<ChatUserCollection>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CurrencyGenerator>().AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx => new CurrencyUpdate(1, ctx.Resolve<ICurrencyGenerator>(), ctx.Resolve<IClock>()))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AutomatedActionSystem>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<RockPaperScissorsGame>().AsSelf().SingleInstance();
            builder.RegisterType<RockPaperScissorsCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<HangmanGame>().AsSelf().SingleInstance();
            builder.RegisterType<HardcodedWordListProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HangmanCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<UptimeCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GiveCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CoinsCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BonusCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StreamsCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ShoutOutCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QuoteCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AliasCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TopBallersCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ScheduleCommand>().AsImplementedInterfaces().SingleInstance();

            builder.Register(ctx => new HelpCommand(ctx.Resolve<IRepository>()))
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();
            builder.Register(ctx => new CommandsCommand(ctx.Resolve<IRepository>()))
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();

            var simpleCommands = repository.List<SimpleCommand>();
            foreach (var command in simpleCommands)
            {
                builder.Register(ctx => command).AsImplementedInterfaces().SingleInstance();
            }

            builder.Register(ctx => new CommandList(ctx.Resolve<IList<IBotCommand>>()))
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<CommandUsageTracker>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CommandHandler>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SubscriberHandler>().AsSelf().SingleInstance();
            builder.RegisterType<FollowableSystem>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<BotMain>().AsImplementedInterfaces().SingleInstance();

            var container = builder.Build();

            WireUpAliasNotifications(container);

            return container;
        }

        private static void WireUpAliasNotifications(IContainer container)
        {
            var commandList = container.Resolve<CommandList>();

            AliasCommand aliasCommand = commandList.OfType<AliasCommand>().SingleOrDefault();

            if (aliasCommand != null)
            {
                foreach (var command in commandList.OfType<BaseCommand>())
                {
                    aliasCommand.CommandAliasModified += (s, e) => command.NotifyWordsModified();
                }
            }
        }
    }
}
