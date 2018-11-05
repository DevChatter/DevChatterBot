using Autofac;

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
        }
    }
}
