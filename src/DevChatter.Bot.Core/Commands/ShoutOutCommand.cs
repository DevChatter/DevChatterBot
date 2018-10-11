using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Core.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Commands
{
    public class ShoutOutCommand : BaseCommand
    {
        private readonly IFollowerService _followerService;

        public ShoutOutCommand(IRepository repository, IFollowerService followerService)
            : base(repository)
        {
            _followerService = followerService;
        }

        protected override async void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string streamName = eventArgs.Arguments?.FirstOrDefault()?.NoAt()
                                ?? await GetRandomFollowedStream();

            chatClient.SendMessage(FormatMessage(streamName));
        }

        private async Task<string> GetRandomFollowedStream()
        {
            IList<string> usersWeFollow = await _followerService.GetUsersWeFollow();
            int randomIndex = MyRandom.RandomNumber(0, usersWeFollow.Count);
            return usersWeFollow[randomIndex];
        }

        private static string FormatMessage(string streamName) =>
            $"Huge shout out to @{streamName} ! You should go check out their channel! https://www.twitch.tv/{streamName}";
    }
}
