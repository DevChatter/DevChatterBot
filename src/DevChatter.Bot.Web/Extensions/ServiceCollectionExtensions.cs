using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace DevChatter.Bot.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ContainerBuilder AddSimpleCommandsFromRepository(
            this ContainerBuilder builder,
            IRepository repository)
        {
            List<SimpleCommand> simpleCommands = repository.List(CommandPolicy.All());

            foreach (SimpleCommand simpleCommand in simpleCommands)
            {
                builder.RegisterInstance(simpleCommand).As<IBotCommand>();
            }

            return builder;
        }

        /// <summary>
        /// Registering the commands for adding, removing, checking, and managing tokens.
        /// </summary>
        public static ContainerBuilder AddCurrencySystem(this ContainerBuilder builder)
        {
            builder.RegisterType<CurrencyGenerator>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CurrencyUpdate>()
                .SingleInstance();
            builder.RegisterType<TaxCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GiveCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CoinsCommand>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BonusCommand>()
                .AsImplementedInterfaces().SingleInstance();

            return builder;
        }

        /// <summary>
        /// Register commands dealing with the stream, shout outs, lists of users and quotes, etc.
        /// </summary>
        public static ContainerBuilder AddStreamMetaCommands(this ContainerBuilder builder)
        {
            builder.RegisterType<RefreshCommandListCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<HypeCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<TopCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<UptimeCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<StreamsCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<ScheduleCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<ShoutOutCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<QuoteCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<ViewersCommand>().As<IBotCommand>().SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddCommandSystem(this ContainerBuilder builder)
        {
            builder.RegisterType<AliasCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<HelpCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<CommandsCommand>().As<IBotCommand>().SingleInstance();
            builder.RegisterType<CommandCooldownTracker>().As<ICommandUsageTracker>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>().SingleInstance();

            return builder;
        }
    }
}
