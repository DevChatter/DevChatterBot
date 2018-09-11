using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Api.Services.Events.FollowerService;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace DevChatter.Bot.Infra.Twitch.Extensions
{
    public static class EventArgsExtensions
    {
        public static WhisperReceivedEventArgs ToWhisperReceivedEventArgs(this OnWhisperReceivedArgs src)
        {
            return new WhisperReceivedEventArgs
            {
                FromDisplayName = src.WhisperMessage.DisplayName,
                Message = src.WhisperMessage.Message
            };
        }

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
                    UserId = commandChatMessage.UserId,
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

            foreach (var followerName in src.NewFollowers.Select(x => x.User.DisplayName))
            {
                eventArgs.FollowerNames.Add(followerName);
            }

            return eventArgs;
        }

        public static UserStatusEventArgs ToUserStatusEventArgs(this OnUserJoinedArgs src)
        {
            return new UserStatusEventArgs
            {
                DisplayName = src.Username,
            };
        }

        public static UserStatusEventArgs ToUserStatusEventArgs(this OnUserLeftArgs src)
        {
            return new UserStatusEventArgs
            {
                DisplayName = src.Username,
            };
        }

        public static UserStatusEventArgs ToUserStatusEventArgs(this OnChatCommandReceivedArgs src)
        {
            return new UserStatusEventArgs
            {
                DisplayName = src.Command.ChatMessage.DisplayName,
            };
        }
    }
}
