using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core.Commands
{
    public class FireworksCommand : BaseCommand
    {
        private readonly IAnimationDisplayNotification _displayNotification;
        public FireworksCommand(IRepository repository, IAnimationDisplayNotification displayNotification)
            : base(repository)
        {
            _displayNotification = displayNotification;
        }

        protected override async void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("Starting some fireworks!");

            await _displayNotification.Fireworks();
        }
    }
}
