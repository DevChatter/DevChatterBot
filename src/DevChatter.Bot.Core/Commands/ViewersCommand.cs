using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core.Commands
{
    public class ViewersCommand : BaseCommand
    {
        private readonly IStreamingPlatform _streamingPlatform;

        public ViewersCommand(IRepository repository, IStreamingPlatform streamingPlatform)
            : base(repository, UserRole.Everyone)
        {
            _streamingPlatform = streamingPlatform;
        }

        protected override async void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            int viewerCount = await _streamingPlatform.GetViewerCountAsync();

            chatClient.SendMessage($"We currently have {viewerCount} viewers!");
        }
    }
}
