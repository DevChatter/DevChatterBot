using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class StreamsCommand : BaseCommand
    {
        private const int MAX_STREAMS = 5;
        private readonly IRepository _repository;

        public StreamsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            _repository = repository;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs.Arguments?.FirstOrDefault();
            string argumentTwo = eventArgs.Arguments?.ElementAtOrDefault(1);
            if (argumentOne != null && argumentOne.Equals("add", StringComparison.InvariantCultureIgnoreCase))
            {
                AddNewStreamer(chatClient, argumentTwo, eventArgs.ChatUser);
            }
            else
            {
                ShoutOutRandomStreamers(chatClient);
            }
        }

        private void ShoutOutRandomStreamers(IChatClient chatClient)
        {
            var toShout = _repository.List<StreamerEntity>().OrderBy(x => Guid.NewGuid()).Take(MAX_STREAMS).ToList();
            if (toShout.Any())
            {
                string streamersText = string.Join(",", toShout.Select(x => $" https://www.twitch.tv/{x.ChannelName} "));
                chatClient.SendMessage($"Huge shoutout to the following streamers ({streamersText})!");
                toShout.ForEach(x => x.TimesShoutedOut++);
                _repository.Update(toShout);
            }
            else
            {
                chatClient.SendMessage("Add some streamers before calling this!");
            }
        }

        private void AddNewStreamer(IChatClient chatClient, string channelName, ChatUser chatUser)
        {
            if (chatUser.CanUserRunCommand(UserRole.Mod))
            {
                if (string.IsNullOrWhiteSpace(channelName))
                {
                    chatClient.SendMessage($"Please specify a valid channel name, @{chatUser.DisplayName}");
                    return;
                }
                // TODO: Prevent inserting same channel
                _repository.Create(new StreamerEntity {ChannelName = channelName});
                chatClient.SendMessage($"Added {channelName} to our list of streams! Thanks, {chatUser.DisplayName} !");
            }
            else
            {
                chatClient.SendMessage($"You aren't allowed to add new streams, @{chatUser.DisplayName}.");
            }
        }
    }
}