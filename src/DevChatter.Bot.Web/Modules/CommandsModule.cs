using Autofac;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.GoogleApi;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Web.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Web.Modules
{
    public class CommandsModule : Autofac.Module
    {
        private readonly IRepository _repository;

        public CommandsModule(IRepository repository)
        {
            _repository = repository;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                Assembly.GetAssembly(typeof(IRepository)),
                Assembly.GetAssembly(typeof(EfGenericRepo)),
                Assembly.GetAssembly(typeof(GoogleApiTimezoneLookup)),
                Assembly.GetAssembly(typeof(TwitchChatClient)),
                Assembly.GetAssembly(typeof(BotHub)),
                Assembly.GetAssembly(typeof(Program)),
            };

            foreach (var assembly in assemblies)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .AssignableTo<BaseCommand>()
                    .As<IBotCommand>()
                    .SingleInstance();
            }

            builder.RegisterType<CommandCooldownTracker>().As<ICommandUsageTracker>().SingleInstance();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>().SingleInstance();

            List<SimpleCommand> simpleCommands = _repository.List(SimpleCommandPolicy.All());

            foreach (SimpleCommand simpleCommand in simpleCommands)
            {
                builder.RegisterInstance(simpleCommand).As<IBotCommand>();
            }

            builder.Register(p => new CommandList(p.Resolve<IList<IBotCommand>>().ToList()));
        }
    }
}
