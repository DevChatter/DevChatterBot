using System;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class StreamsCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        public StreamsCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "Streams";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs.Arguments?.FirstOrDefault();
            string argumentTwo = eventArgs.Arguments?.ElementAtOrDefault(1);
            if (argumentOne == "add")
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
            var toShout = _repository.List<StreamerEntity>().OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            string streamersText = string.Join(",", toShout.Select(x => $" https://www.twitch.tv/{x.ChannelName} "));
            chatClient.SendMessage($"Huge shoutout to the following streamers ({streamersText})!");
            toShout.ForEach(x => x.TimesShoutedOut++);
            _repository.Update(toShout);
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
            }
            else
            {
                chatClient.SendMessage($"You aren't allowed to add new streams, @{chatUser.DisplayName}.");
            }
        }
    }
}