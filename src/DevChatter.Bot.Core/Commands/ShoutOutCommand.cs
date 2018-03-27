using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Model;
using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class ShoutOutCommand : SimpleCommand
    {
        private readonly IFollowerService _followerService;

        public ShoutOutCommand(IFollowerService followerService)
        {
            CommandText = "so";
            _followerService = followerService;
            RoleRequired = UserRole.Mod;
        }

        public override void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            string streamName = eventArgs.Arguments?.FirstOrDefault()?.NoAt() ?? GetRandomFollowedStream();

            triggeringClient.SendMessage(FormatMessage(streamName));
        }

        private string GetRandomFollowedStream()
        {
            List<string> usersWeFollow = _followerService.GetUsersWeFollow();
            int randomIndex = MyRandom.RandomNumber(0, usersWeFollow.Count);
            return usersWeFollow[randomIndex];
        }

        private static string FormatMessage(string streamName) => $"Huge shout out to @{streamName} ! You should go check out their channel! https://www.twitch.tv/{streamName}";
    }
}