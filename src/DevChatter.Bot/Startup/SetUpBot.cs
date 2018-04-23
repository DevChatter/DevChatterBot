using System.Linq;
using Autofac;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Infra.Twitch;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        public static IContainer NewBotDependencyContainer(BotConfiguration botConfiguration)
        {
            var repository = SetUpDatabase.SetUpRepository(botConfiguration.DatabaseConnectionString);

            var builder = new ContainerBuilder();

            builder.RegisterModule<DevChatterBotCoreModule>();
            builder.RegisterModule<DevChatterBotTwitchModule>();

            builder.Register(ctx => botConfiguration.CommandHandlerSettings).AsSelf().SingleInstance();
            builder.Register(ctx => botConfiguration.TwitchClientSettings).AsSelf().SingleInstance();

            builder.RegisterType<ConsoleChatClient>().AsImplementedInterfaces().SingleInstance();

            builder.Register(ctx => repository)
                   .As<IRepository>().SingleInstance();

            var simpleCommands = repository.List<SimpleCommand>();
            foreach (var command in simpleCommands)
            {
                builder.Register(ctx => command)
                       .AsImplementedInterfaces()
                       .SingleInstance();
            }
            
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
