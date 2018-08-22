using Autofac;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using System.Collections.Generic;

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

            return builder;
        }

        public static ContainerBuilder AddCommandSystem(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandCooldownTracker>().As<ICommandUsageTracker>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>().SingleInstance();

            return builder;
        }
    }
}
