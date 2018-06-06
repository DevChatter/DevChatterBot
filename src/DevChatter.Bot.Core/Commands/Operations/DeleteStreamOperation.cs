using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DeleteStreamOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;

        public DeleteStreamOperation(IRepository repository)
        {
            _repository = repository;
        }

        public override List<string> OperandWords { get; } = new List<string> {"del", "rem", "delete", "remove", "rm"};
        public override string HelpText { get; } = "";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            var chatUser = eventArgs.ChatUser;
            string channelName = eventArgs.Arguments?.ElementAtOrDefault(1);
            if (chatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
                if (string.IsNullOrWhiteSpace(channelName))
                {
                    return $"Please specify a valid channel name, @{chatUser.DisplayName}";
                }

                StreamerEntity entity = _repository.Single(StreamerEntityPolicy.ByChannel(channelName));
                if (entity != null)
                {
                    _repository.Remove(entity);
                    return $"Removed {channelName} from our list of streams!";
                }

                return $"We don't have {channelName} in our list of streams! Check the spelling?";
            }

            return $"You aren't allowed to remove streams, @{chatUser.DisplayName}.";
        }
    }
}
