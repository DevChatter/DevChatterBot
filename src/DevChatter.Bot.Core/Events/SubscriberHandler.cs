using System.Collections.Generic;

namespace DevChatter.Bot.Core.Events
{
    public class SubscriberHandler
    {
        public SubscriberHandler(List<IChatClient> chatClients)
        {
            foreach (IChatClient chatClient in chatClients)
            {
                chatClient.OnNewSubscriber += ChatClientOnOnNewSubscriber;
            }
        }

        private void ChatClientOnOnNewSubscriber(object sender, NewSubscriberEventArgs newSubscriberEventArgs)
        {
            if (sender is IChatClient chatClient)
            {
                chatClient.SendMessage($"Welcome, {newSubscriberEventArgs.SubscriberName}! You are awesome! Thank you for subscribing!");
            }
        }
    }
}