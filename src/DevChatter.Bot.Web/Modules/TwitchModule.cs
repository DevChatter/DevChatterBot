using Autofac;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;

namespace DevChatter.Bot.Web.Modules
{
    public class TwitchModule : Module
    {
        private readonly TwitchClientSettings _twitchClientSettings;

        public TwitchModule(TwitchClientSettings twitchClientSettings)
        {
            _twitchClientSettings = twitchClientSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TwitchSubscriberHandler>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TwitchFollowerService>()
                .WithParameter("settings", _twitchClientSettings)
                .As<IFollowerService>().SingleInstance();

            var api = new TwitchAPI();
            api.Settings.ClientId = _twitchClientSettings.TwitchClientId;
            api.Settings.AccessToken = _twitchClientSettings.TwitchBotOAuth;

            builder.RegisterInstance(api).As<ITwitchAPI>().SingleInstance();

            builder.RegisterType<TwitchChatClient>()
                .WithParameter("settings", _twitchClientSettings)
                .As<IChatClient>().AsSelf().SingleInstance();

            builder.RegisterType<TwitchStreamingInfoService>()
                .As<IStreamingInfoService>().SingleInstance();

        }

    }
}
