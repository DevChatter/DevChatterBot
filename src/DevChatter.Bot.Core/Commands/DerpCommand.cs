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
        private readonly IAnimationDisplayNotification _animationDisplayNotification;

        public DerpCommand(IRepository repository,
            IAnimationDisplayNotification animationDisplayNotification)
            : base(repository, UserRole.Everyone)
        {
            _animationDisplayNotification = animationDisplayNotification;
            Cooldown = TimeSpan.FromMinutes(5);
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("Derp derp devchaDerp !");
            _animationDisplayNotification.Derp();
        }
    }
}
