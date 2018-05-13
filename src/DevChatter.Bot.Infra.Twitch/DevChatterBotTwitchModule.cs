using Autofac;
using DevChatter.Bot.Infra.Twitch.Events;
using TwitchLib.Api;

namespace DevChatter.Bot.Infra.Twitch
{
    public class DevChatterBotTwitchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TwitchFollowerService>().AsImplementedInterfaces().SingleInstance();

            builder.Register(ctx =>
                {
                    var api = new TwitchAPI();
                    var settings = ctx.Resolve<TwitchClientSettings>();
                    api.Settings.ClientId = settings.TwitchClientId;
                    api.Settings.AccessToken = settings.TwitchChannelOAuth; // need to verify this as well
                    return api;
                })
                   .AsImplementedInterfaces();

            builder.RegisterType<TwitchChatClient>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TwitchStreamingInfoService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
