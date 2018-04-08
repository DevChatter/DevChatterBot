using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Commands
{
    public class ShoutOutCommand : BaseCommand
    {
        private readonly IFollowerService _followerService;

        public ShoutOutCommand(IRepository repository, IFollowerService followerService)
            : base(repository, UserRole.Mod)
        {
            _followerService = followerService;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string streamName = eventArgs.Arguments?.FirstOrDefault()?.NoAt() ?? GetRandomFollowedStream();

            chatClient.SendMessage(FormatMessage(streamName));
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