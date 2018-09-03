using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public class FollowableSystem : IFollowableSystem //, IChatSystem
    {
        private const int TOKENS_FOR_FOLLOWING = 100;

        private readonly IChatClient _chatClient;
        private readonly IFollowerService _followerService;
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly ISubscriberHandler _subscriberHandler;

        public FollowableSystem(IChatClient chatClient, IFollowerService followerService,
            ICurrencyGenerator currencyGenerator, ISubscriberHandler subscriberHandler)
        {
            _chatClient = chatClient;
            _followerService = followerService;
            _currencyGenerator = currencyGenerator;
            _subscriberHandler = subscriberHandler;
        }

        public void HandleFollowerNotifications()
        {
            _followerService.OnNewFollower += FollowerServiceOnOnNewFollower;
            _subscriberHandler.OnNewSubscriber += SubscriberHandlerOnOnNewSubscriber;
        }

        private void SubscriberHandlerOnOnNewSubscriber(object sender, NewSubscriberEventArgs e)
        {
            // We need to figure out how to handle subscribe services without a chat client
            _chatClient?.SendMessage(
                    $"Welcome, {e.SubscriberName}! You are awesome! Thank you for supporting us!");
        }

        private void FollowerServiceOnOnNewFollower(object sender, NewFollowersEventArgs eventArgs)
        {
            foreach (string followerName in eventArgs.FollowerNames)
            {
                _currencyGenerator.AddCurrencyTo(followerName, TOKENS_FOR_FOLLOWING);
                _chatClient.SendMessage(
                    $"Welcome, {followerName}! Thank you for following! {TOKENS_FOR_FOLLOWING} coins to have some fun. Everyone, say \"hello\"!");
            }
        }

        public void StopHandlingNotifications()
        {
            _followerService.OnNewFollower -= FollowerServiceOnOnNewFollower;
        }
    }
}
