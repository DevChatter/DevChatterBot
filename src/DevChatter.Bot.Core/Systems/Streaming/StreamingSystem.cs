using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public class StreamingSystem : IStreamingPlatform
    {
        private const int TOKENS_FOR_FOLLOWING = 100;

        private readonly IChatClient _chatClient;
        private readonly IFollowerService _followerService;
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly ISubscriberHandler _subscriberHandler;
        private readonly IStreamingInfoService _streamingInfoService;

        public StreamingSystem(IChatClient chatClient, IFollowerService followerService,
            ICurrencyGenerator currencyGenerator, ISubscriberHandler subscriberHandler,
            IStreamingInfoService streamingInfoService)
        {
            _chatClient = chatClient;
            _followerService = followerService;
            _currencyGenerator = currencyGenerator;
            _subscriberHandler = subscriberHandler;
            _streamingInfoService = streamingInfoService;
        }

        public async Task Connect()
        {
            await _chatClient.Connect();
            _chatClient.SendMessage("Hello World! The bot has arrived!");
            _followerService.OnNewFollower += FollowerServiceOnOnNewFollower;
            _subscriberHandler.OnNewSubscriber += SubscriberHandlerOnOnNewSubscriber;
        }

        public async Task Disconnect()
        {
            _chatClient.SendMessage("Goodbye for now! The bot has left the building...");
            _followerService.OnNewFollower -= FollowerServiceOnOnNewFollower;
            _subscriberHandler.OnNewSubscriber -= SubscriberHandlerOnOnNewSubscriber;
            await _chatClient.Disconnect();
        }

        private void SubscriberHandlerOnOnNewSubscriber(
            object sender, NewSubscriberEventArgs e)
        {
            _chatClient.SendMessage($"Welcome, {e.SubscriberName}! You are awesome! Thank you for supporting us!");
        }

        private void FollowerServiceOnOnNewFollower(
            object sender, NewFollowersEventArgs eventArgs)
        {
            foreach (string followerName in eventArgs.FollowerNames)
            {
                _currencyGenerator.AddCurrencyTo(followerName, TOKENS_FOR_FOLLOWING);
                _chatClient.SendMessage($"Welcome, {followerName}! Thank you for following! {TOKENS_FOR_FOLLOWING} coins to have some fun. Everyone, say \"hello\"!");
            }
        }

        public Task<TimeSpan?> GetUptimeAsync()
        {
            return _streamingInfoService.GetUptimeAsync();
        }

        public Task<int> GetViewerCountAsync()
        {
            return _streamingInfoService.GetViewerCountAsync();
        }

    }
}
