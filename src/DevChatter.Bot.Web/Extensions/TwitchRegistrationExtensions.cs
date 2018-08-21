using Autofac;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;

namespace DevChatter.Bot.Web.Extensions
{
    public static class TwitchRegistrationExtensions
    {
        public static ContainerBuilder AddTwitchLibConnection(
            this ContainerBuilder builder,
            TwitchClientSettings twitchClientSettings)
        {
            builder.RegisterType<SubscriberHandler>()
                .AsSelf().SingleInstance();

            builder.RegisterType<FollowableSystem>()
                .As<IFollowableSystem>().SingleInstance();

            builder.RegisterType<TwitchFollowerService>()
                .WithParameter("settings", twitchClientSettings)
                .As<IFollowerService>().SingleInstance();

            var api = new TwitchAPI();
            api.Settings.ClientId = twitchClientSettings.TwitchClientId;
            api.Settings.AccessToken = twitchClientSettings.TwitchChannelOAuth;

            builder.RegisterInstance(api).As<ITwitchAPI>().SingleInstance();

            builder.RegisterType<TwitchChatClient>()
                .WithParameter("settings", twitchClientSettings)
                .As<IChatClient>().SingleInstance();

            builder.RegisterType<TwitchStreamingInfoService>()
                .As<IStreamingInfoService>().SingleInstance();

            return builder;
        }
    }
}
