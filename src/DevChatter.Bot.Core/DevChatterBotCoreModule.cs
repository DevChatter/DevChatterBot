using System.Collections.Generic;
using Autofac;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.Heist;
using DevChatter.Bot.Core.Games.Quiz;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Core.Util;
using Microsoft.Extensions.Logging;

namespace DevChatter.Bot.Core
{
    public class DevChatterBotCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StreamingPlatform>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<SystemClock>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterGeneric(typeof(LoggerAdapter<>)).AsImplementedInterfaces();

            builder.RegisterType<ChatUserCollection>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CurrencyGenerator>().AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx => new CurrencyUpdate(1, ctx.Resolve<ICurrencyGenerator>(), ctx.Resolve<IClock>()))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AutomatedActionSystem>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<RockPaperScissorsGame>().AsSelf().SingleInstance();
            builder.RegisterType<RockPaperScissorsCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<HeistCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HeistGame>().AsSelf().SingleInstance();

            builder.RegisterType<QuizCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QuizGame>().AsSelf().SingleInstance();


            builder.RegisterType<HangmanGame>().AsSelf().SingleInstance();
            builder.RegisterType<HangmanCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TopCommand>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<UptimeCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TaxCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GiveCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CoinsCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BonusCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StreamsCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ShoutOutCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QuoteCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AliasCommand>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ScheduleCommand>().AsImplementedInterfaces().SingleInstance();

            builder.Register(ctx => new HelpCommand(ctx.Resolve<IRepository>()))
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();
            builder.Register(ctx => new CommandsCommand(ctx.Resolve<IRepository>()))
                .OnActivated(e => e.Instance.AllCommands = e.Context.Resolve<CommandList>())
                .AsImplementedInterfaces();

            builder.Register(ctx => new CommandList(ctx.Resolve<IList<IBotCommand>>()))
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<CommandCooldownTracker>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CommandHandler>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SubscriberHandler>().AsSelf().SingleInstance();
            builder.RegisterType<FollowableSystem>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<BotMain>().AsSelf().SingleInstance();
        }
    }
}
