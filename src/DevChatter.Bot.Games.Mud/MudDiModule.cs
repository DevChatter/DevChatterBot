using System.Reflection;
using Autofac;
using DevChatter.Bot.Core.Handlers;
using DevChatter.Bot.Games.Mud.Actions;
using Module = Autofac.Module;

namespace DevChatter.Bot.Games.Mud
{
    public class MudDiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MudRoomHandler>()
                .As<IBotHostedHandler>();

            builder.RegisterType<MudGame>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IMudAction)))
                .AssignableTo<BaseMudAction>()
                .As<IMudAction>()
                .SingleInstance();
        }
    }
}
