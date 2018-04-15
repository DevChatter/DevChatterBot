using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class StreamsCommand : BaseCommand
    {
        private const int MAX_STREAMS = 5;
        private readonly List<BaseCommandOperation> _operations;

        public StreamsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
            HelpText = $"Use \"!{PrimaryCommandText}\" to shout out the streams we like! To add a new one use \"!{PrimaryCommandText} add channelName\", but it only works for mods.";
            _operations = new List<BaseCommandOperation>
            {
                new AddStreamOperation(repository),
                new DeleteStreamOperation(repository),
            };
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var oper = eventArgs?.Arguments?.ElementAtOrDefault(0)?.ToLowerInvariant();

            var operationToUse = _operations.SingleOrDefault(x => x.ShouldExecute(oper));
            if (operationToUse != null)
            {
                string resultMessage = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(resultMessage);
            }
            else
            {
                ShoutOutRandomStreamers(chatClient);
            }
        }

        private void ShoutOutRandomStreamers(IChatClient chatClient)
        {
            var toShout = Repository.List<StreamerEntity>().OrderBy(x => Guid.NewGuid()).Take(MAX_STREAMS).ToList();
            if (toShout.Any())
            {
                string streamersText =
                    string.Join(",", toShout.Select(x => $" https://www.twitch.tv/{x.ChannelName} "));
                chatClient.SendMessage($"Huge shoutout to the following streamers ({streamersText})!");
                toShout.ForEach(x => x.TimesShoutedOut++);
                Repository.Update(toShout);
            }
            else
            {
                chatClient.SendMessage("Add some streamers before calling this!");
            }
        }
    }
}
