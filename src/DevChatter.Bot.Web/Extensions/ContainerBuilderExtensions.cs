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
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddSimpleCommandsFromRepository(
            this ContainerBuilder builder,
            IRepository repository)
        {
            List<SimpleCommand> simpleCommands = repository.List(SimpleCommandPolicy.All());

            foreach (SimpleCommand simpleCommand in simpleCommands)
            {
                builder.RegisterInstance(simpleCommand).As<IBotCommand>();
            }

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
