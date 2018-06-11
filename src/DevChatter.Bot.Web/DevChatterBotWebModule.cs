using Autofac;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Infra.Ef;
using DevChatter.Bot.Infra.Twitch;
using Microsoft.Extensions.Logging;

namespace DevChatter.Bot.Web
{
    public class DevChatterBotWebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();

            builder.RegisterModule<DevChatterBotCoreModule>();
            builder.RegisterModule<DevChatterBotTwitchModule>();

            builder.RegisterType<EfGenericRepo>().As<IRepository>().SingleInstance();
        }
    }
}
