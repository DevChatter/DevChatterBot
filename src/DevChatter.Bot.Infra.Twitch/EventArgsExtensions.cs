using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;
using TwitchLib.Events.Client;
using TwitchLib.Events.Services.FollowerService;
using TwitchLib.Models.Client;

namespace DevChatter.Bot.Infra.Twitch
{
    public static class EventArgsExtensions
    {
        public static CommandReceivedEventArgs ToCommandReceivedEventArgs(this OnChatCommandReceivedArgs src)
        {
            ChatMessage commandChatMessage = src.Command.ChatMessage;
            UserRole topRole = GetTopRole(commandChatMessage);

            var eventArgs = new CommandReceivedEventArgs
            {
                CommandWord = src.Command.CommandText,
                Arguments = src.Command.ArgumentsAsList,
                ChatUser = new ChatUser
                {
                    DisplayName = commandChatMessage.DisplayName,
                    Role = topRole
                }
            };

            return eventArgs;
        }

        private static UserRole GetTopRole(ChatMessage commandChatMessage)
        {
            var userRoles = new List<UserRole> {UserRole.Everyone};
            if (commandChatMessage.IsBroadcaster)
            {
                return UserRole.Streamer;
            }

            if (commandChatMessage.IsModerator)
            {
                return UserRole.Mod;
            }

            if (commandChatMessage.IsSubscriber)
            {
                return UserRole.Subscriber;
            }

            return UserRole.Everyone;
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