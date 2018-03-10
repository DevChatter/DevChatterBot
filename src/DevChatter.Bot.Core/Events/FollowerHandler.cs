using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public class FollowerHandler
    {
        public FollowerHandler(List<IChatClient> chatClients)
        {
            foreach (IChatClient chatClient in chatClients)
            {
                chatClient.OnNewFollower += ChatClientOnOnNewFollower;
            }
        }

        private void ChatClientOnOnNewFollower(object sender, NewFollowersEventArgs eventArgs)
        {
            if (sender is IChatClient chatClient)
            {
                foreach (string followerName in eventArgs.FollowerNames)
                {
                    chatClient.SendMessage($"Welcome, {followerName}!");
                }
            }
        }
    }
}