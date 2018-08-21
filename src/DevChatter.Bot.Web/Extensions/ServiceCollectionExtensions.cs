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
        public static IServiceCollection AddSimpleCommandsFromRepository(
            this IServiceCollection services,
            IRepository repository)
        {
            List<SimpleCommand> simpleCommands = repository.List(CommandPolicy.All());

            foreach (SimpleCommand simpleCommand in simpleCommands)
            {
                services.AddSingleton<IBotCommand>(simpleCommand);
            }

            return services;
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
        public static IServiceCollection AddStreamMetaCommands(this IServiceCollection services)
        {
            services.AddSingleton<IBotCommand, HypeCommand>();
            services.AddSingleton<IBotCommand, RefreshCommandListCommand>();
            services.AddSingleton<IBotCommand, TopCommand>();
            services.AddSingleton<IBotCommand, UptimeCommand>();
            services.AddSingleton<IBotCommand, StreamsCommand>();
            services.AddSingleton<IBotCommand, ScheduleCommand>();
            services.AddSingleton<IBotCommand, ShoutOutCommand>();
            services.AddSingleton<IBotCommand, QuoteCommand>();
            services.AddSingleton<IBotCommand, ViewersCommand>();

            return services;
        }

        public static IServiceCollection AddCommandSystem(this IServiceCollection services)
        {
            services.AddSingleton<IBotCommand, AliasCommand>();
            services.AddSingleton<IBotCommand, HelpCommand>();
            services.AddSingleton<IBotCommand, CommandsCommand>();

            //services.AddSingleton(p => new CommandList(p.GetServices<IBotCommand>().ToList(), p));

            services.AddSingleton<ICommandUsageTracker, CommandCooldownTracker>();
            services.AddSingleton<ICommandHandler, CommandHandler>();


            return services;
        }
    }
}
