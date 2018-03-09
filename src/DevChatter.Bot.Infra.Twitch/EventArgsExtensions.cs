using DevChatter.Bot.Core;
using TwitchLib.Events.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public static class EventArgsExtensions
    {
        public static CommandReceivedEventArgs ToCommandReceivedEventArgs(this OnChatCommandReceivedArgs src)
        {
            var eventArgs = new CommandReceivedEventArgs
            {
                CommandWord = src.Command.CommandText,
            };

            return eventArgs;
        }

        public static NewSubscriberEventArgs ToNewSubscriberEventArgs(this OnNewSubscriberArgs src)
        {
            var eventArgs = new NewSubscriberEventArgs
            {
                SubscriberName = src.Subscriber.DisplayName,
            };

            return eventArgs;
        }
    }
}