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

            builder.RegisterType<SurvivorRepo>()
                .AsSelf();

            builder.RegisterType<WastefulMoveCommand>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TeamCommand>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ShopCommand>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<WastefulDisplayNotification>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<InventoryCommand>()
                .AsImplementedInterfaces().SingleInstance();

            // TODO: Remove this connection string!!!!
            IGameRepository repo = SetUpGameDatabase.SetUpRepository("Server=(localdb)\\mssqllocaldb;Database=WastefulGame;Trusted_Connection=True;MultipleActiveResultSets=true");

            builder.RegisterInstance(repo)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
