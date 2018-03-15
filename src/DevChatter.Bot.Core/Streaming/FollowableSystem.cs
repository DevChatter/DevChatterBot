using System.Collections.Generic;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Streaming
{
    public class FollowableSystem
    {
        private readonly IList<IChatClient> _chatClients;
        private readonly IFollowerService _followerService;

        public FollowableSystem(IList<IChatClient> chatClients, IFollowerService followerService)
        {
            _chatClients = chatClients;
            _followerService = followerService;
        }

        public void HandleFollowerNotifications()
        {
            _followerService.OnNewFollower += FollowerServiceOnOnNewFollower;
        }

        private void FollowerServiceOnOnNewFollower(object sender, NewFollowersEventArgs eventArgs)
        {
            foreach (IChatClient chatClient in _chatClients)
            {
                foreach (string followerName in eventArgs.FollowerNames)
                {
                    chatClient.SendMessage($"Welcome, {followerName}! Thank you for following! Everyone, say \"hello\"!");
                }
            }
        }
    }
}