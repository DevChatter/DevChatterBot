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
        public static IServiceCollection AddTwitchLibConnection(this IServiceCollection services,
            TwitchClientSettings twitchClientSettings)
        {
            services.AddSingleton<SubscriberHandler>();

            services.AddSingleton<IFollowableSystem, FollowableSystem>();

            services.AddSingleton<IFollowerService, TwitchFollowerService>();

            var api = new TwitchAPI();
            api.Settings.ClientId = twitchClientSettings.TwitchClientId;
            api.Settings.AccessToken = twitchClientSettings.TwitchChannelOAuth;
            services.AddSingleton<ITwitchAPI>(api);

            services.AddSingleton<IChatClient, TwitchChatClient>();

            services.AddSingleton<IStreamingInfoService, TwitchStreamingInfoService>();

            return services;
        }
    }
}
