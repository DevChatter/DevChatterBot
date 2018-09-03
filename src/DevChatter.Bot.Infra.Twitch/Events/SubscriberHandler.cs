using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Events
{
    public class SubscriberHandler : ISubscriberHandler
    {
        private readonly ILoggerAdapter<SubscriberHandler> _logger;

        public SubscriberHandler(IList<IChatClient> chatClients, ILoggerAdapter<SubscriberHandler> logger)
        {
            _logger = logger;
            // This needing a chat client is twitch specific! Get rid of it!
            foreach (IChatClient chatClient in chatClients)
            {
                chatClient.OnNewSubscriber += ChatClientOnOnNewSubscriber;
            }
        }

        private void ChatClientOnOnNewSubscriber(object sender, NewSubscriberEventArgs eventArgs)
        {
            OnNewSubscriber?.Invoke(sender, eventArgs);
        }

        public event EventHandler<NewSubscriberEventArgs> OnNewSubscriber;
    }
}
