using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class RefreshCommandListCommand : BaseCommand
    {
        private CommandList _commandList;

        public RefreshCommandListCommand(IRepository repository)
            : base(repository)
        {
        }

        public bool NeedsInitializing { get; private set; } = true;

        public void Initialize(CommandList commandList)
        {
            _commandList = commandList;
            NeedsInitializing = false;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (NeedsInitializing)
            {
                chatClient.SendMessage("Refresh command not configured correctly.");
            }
            else
            {
                chatClient.SendMessage("Gonna try a refresh.");
                _commandList.Refresh();
                chatClient.SendMessage("Should be refreshed now.");
            }
        }
    }
}
