using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;

namespace DevChatter.Bot.Core.Commands
{
    public class HypeCommand : BaseCommand
    {
        private readonly IOverlayNotification _overlayNotification;

        public HypeCommand(IRepository repository,
            IOverlayNotification overlayNotification)
            : base(repository, UserRole.Everyone)
        {
            _overlayNotification = overlayNotification;
            Cooldown = TimeSpan.FromMinutes(2);
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("Hype hype devchaHype !");
            _overlayNotification.Hype();
        }
    }
}
