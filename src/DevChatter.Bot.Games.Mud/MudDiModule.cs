using Autofac;
using DevChatter.Bot.Core.Handlers;

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
        }
    }
}
