using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core.Commands.Overlays
{
    public class TopicCommand : BaseCommand
    {
        private readonly IOverlayDisplayNotification _overlayDisplay;

        public TopicCommand(IRepository repository,
            IOverlayDisplayNotification overlayDisplay)
            : base(repository)
        {
            _overlayDisplay = overlayDisplay;
        }

        protected override void HandleCommand(IChatClient chatClient,
            CommandReceivedEventArgs eventArgs)
        {
            string newTopic = string.Join(" ", eventArgs.Arguments).Trim();

            _overlayDisplay.ChangeTopic(newTopic);
        }
    }
}
