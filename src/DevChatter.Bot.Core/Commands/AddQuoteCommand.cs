using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Commands
{
    public class AddQuoteCommand : SimpleCommand
    {
        private readonly IRepository _repository;
        public AddQuoteCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "quoteadd";
            RoleRequired = UserRole.Mod;
        }

        public override void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            triggeringClient.SendMessage("You are trying to add a quote, but that feature isn't supported yet!");
        }
    }
}