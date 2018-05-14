using System.Collections.Generic;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Events
{
    public class SubscriberHandler
    {
        private readonly ILoggerAdapter<SubscriberHandler> _logger;

        public SubscriberHandler(IList<IChatClient> chatClients, ILoggerAdapter<SubscriberHandler> logger)
        {
            _logger = logger;
            foreach (IChatClient chatClient in chatClients)
            {
                chatClient.OnNewSubscriber += ChatClientOnOnNewSubscriber;
            }
        }

        private void ChatClientOnOnNewSubscriber(object sender, NewSubscriberEventArgs eventArgs)
        {
            _logger.LogInformation($"Running {nameof(ChatClientOnOnNewSubscriber)}()");
            if (sender is IChatClient chatClient)
            {
                chatClient.SendMessage(
                    $"Welcome, {eventArgs.SubscriberName}! You are awesome! Thank you for subscribing!");
            }
        }
    }
}
