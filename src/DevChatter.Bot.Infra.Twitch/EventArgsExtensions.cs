using System.Linq;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;
using TwitchLib.Events.Client;
using TwitchLib.Events.Services.FollowerService;

namespace DevChatter.Bot.Infra.Twitch
{
    public static class EventArgsExtensions
    {
        public static CommandReceivedEventArgs ToCommandReceivedEventArgs(this OnChatCommandReceivedArgs src)
        {
            var eventArgs = new CommandReceivedEventArgs
            {
                CommandWord = src.Command.CommandText,
                Arguments = src.Command.ArgumentsAsList,
                ChatUser = new ChatUser { DisplayName = src.Command.ChatMessage.DisplayName }
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

        public static NewFollowersEventArgs ToNewFollowerEventArgs(this OnNewFollowersDetectedArgs src)
        {
            var eventArgs = new NewFollowersEventArgs();

            eventArgs.FollowerNames.AddRange(src.NewFollowers.Select(x => x.User.DisplayName));

            return eventArgs;
        }
    }
}