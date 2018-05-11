using Autofac;
using DevChatter.Bot.Infra.Twitch.Events;
using TwitchLib;

namespace DevChatter.Bot.Infra.Twitch
{
    public class DevChatterBotTwitchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TwitchFollowerService>().AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx => new TwitchAPI(ctx.Resolve<TwitchClientSettings>().TwitchClientId))
                .AsImplementedInterfaces();
            builder.RegisterType<TwitchChatClient>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TwitchStreamingInfoService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
