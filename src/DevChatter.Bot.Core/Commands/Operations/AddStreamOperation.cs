using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class AddStreamOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public AddStreamOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; } = new List<string> { "add" };
        public override string HelpText { get; } = "";
        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var chatUser = eventArgs.ChatUser;
            string channelName = eventArgs.Arguments?.ElementAtOrDefault(1);
            if (chatUser.CanUserRunCommand(UserRole.Mod))
            {
                if (string.IsNullOrWhiteSpace(channelName))
                {
                    return $"Please specify a valid channel name, @{chatUser.DisplayName}";
                }

                StreamerEntity entity = _repository.Single(StreamerEntityPolicy.ByChannel(channelName));
                if (entity == null)
                {
                    _repository.Create(new StreamerEntity { ChannelName = channelName });
                    return $"Added {channelName} to our list of streams! Thanks, {chatUser.DisplayName} !";
                }

                return $"We already have {channelName} in our list of streams!";
            }

            return $"You aren't allowed to add new streams, @{chatUser.DisplayName}.";
        }
    }
}
