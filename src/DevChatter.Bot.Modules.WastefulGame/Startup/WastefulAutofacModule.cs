using Autofac;
using DevChatter.Bot.Modules.WastefulGame.Commands;
using DevChatter.Bot.Modules.WastefulGame.Data;

namespace DevChatter.Bot.Modules.WastefulGame.Startup
{
    public class WastefulAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WastefulStartCommand>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<WastefulMoveCommand>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<WastefulDisplayNotification>()
                .AsImplementedInterfaces().SingleInstance();

            IGameRepository repo = SetUpGameDatabase.SetUpRepository("nope");

            builder.RegisterInstance(repo)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
