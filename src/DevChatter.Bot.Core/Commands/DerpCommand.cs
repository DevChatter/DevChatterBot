using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System;

namespace DevChatter.Bot.Core.Commands
{
    public class DerpCommand : BaseCommand
    {
        private readonly IOverlayNotification _overlayNotification;

        public DerpCommand(IRepository repository,
            IOverlayNotification overlayNotification)
            : base(repository, UserRole.Everyone)
        {
            _overlayNotification = overlayNotification;
            Cooldown = TimeSpan.FromMinutes(5);
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("Derp derp devchaDerp !");
            _overlayNotification.Derp();
        }
    }
}
