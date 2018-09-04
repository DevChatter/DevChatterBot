using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using System;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    public class TwitchSubscriberHandler : ISubscriberHandler
    {
        public TwitchSubscriberHandler(TwitchChatClient chatClient)
        {
            chatClient.OnNewSubscriber += ChatClientOnOnNewSubscriber;
        }

        private void ChatClientOnOnNewSubscriber(object sender, NewSubscriberEventArgs eventArgs)
        {
            OnNewSubscriber?.Invoke(sender, eventArgs);
        }

        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
    }
}
