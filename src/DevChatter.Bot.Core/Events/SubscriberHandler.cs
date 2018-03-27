using System.Collections.Generic;
using DevChatter.Bot.Core.Systems.Chat;

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

        private void ChatClientOnOnNewSubscriber(object sender, NewSubscriberEventArgs eventArgs)
        {
            if (sender is IChatClient chatClient)
            {
                chatClient.SendMessage($"Welcome, {eventArgs.SubscriberName}! You are awesome! Thank you for subscribing!");
            }
        }
    }
}