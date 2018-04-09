using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Autofac;
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
        public static IContainer NewBotDepedencyContainer(BotConfiguration botConfiguration)
        {
            var repository = SetUpDatabase.SetUpRepository(botConfiguration.DatabaseConnectionString);

            var builder = new ContainerBuilder();

            builder.Register(ctx => botConfiguration.CommandHandlerSettings).AsSelf().InstancePerLifetimeScope();
            builder.Register(ctx => botConfiguration.TwitchClientSettings).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<TwitchFollowerService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.Register(ctx => new TwitchAPI(botConfiguration.TwitchClientSettings.TwitchClientId))
                .AsImplementedInterfaces();
            builder.RegisterType<TwitchChatClient>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<StreamingPlatform>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<TwitchStreamingInfoService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<ConsoleChatClient>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Register(ctx => repository).As<IRepository>().InstancePerLifetimeScope();

            builder.RegisterType<ChatUserCollection>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyGenerator>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.Register(ctx => new CurrencyUpdate(1, ctx.Resolve<ICurrencyGenerator>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<AutomatedActionSystem>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<RockPaperScissorsGame>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RockPaperScissorsCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<HangmanGame>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<HardcodedWordListProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<HangmanCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<UptimeCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GiveCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CoinsCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<BonusCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<StreamsCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ShoutOutCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<QuoteCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<AddQuoteCommand>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Register(ctx => new HelpCommand())
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();
            builder.Register(ctx => new CommandsCommand())
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();
            builder.Register(ctx => new AddCommandCommand(ctx.Resolve<IRepository>()))
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();
            builder.Register(ctx => new RemoveCommandCommand(ctx.Resolve<IRepository>()))
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();

            var simpleCommands = repository.List<SimpleCommand>();
            foreach (var command in simpleCommands)
            {
                builder.Register(ctx => command).AsImplementedInterfaces().InstancePerLifetimeScope();
            }

            builder.Register(ctx => new CommandList(ctx.Resolve<IList<IBotCommand>>()))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandUsageTracker>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CommandHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberHandler>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<FollowableSystem>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<BotMain>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var container = builder.Build();

            return container;
        }

    }
}