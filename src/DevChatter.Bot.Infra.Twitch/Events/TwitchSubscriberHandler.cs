using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System;

namespace DevChatter.Bot.Infra.Twitch.Events
{
    // TODO: Maybe rename this to "ChatBasedSubscriberHandler"
    public class TwitchSubscriberHandler : ISubscriberHandler
    {
        public TwitchSubscriberHandler(IChatClient chatClient)
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
