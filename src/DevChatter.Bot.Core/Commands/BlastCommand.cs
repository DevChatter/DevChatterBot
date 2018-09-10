using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Systems.Streaming;
using System;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class BlastCommand : BaseCommand
    {
        private readonly IAnimationDisplayNotification _animationDisplayNotification;

        public BlastCommand(IRepository repository,
            IAnimationDisplayNotification animationDisplayNotification)
            : base(repository, UserRole.Everyone)
        {
            _animationDisplayNotification = animationDisplayNotification;
            Cooldown = TimeSpan.FromMinutes(2);
            string choiceText = "\"hype\" or \"derp\""; // create from operations later.
            HelpText = $"Use \"!blast [choice]\". Replace [choice] with one of: {choiceText} ";
        }

        protected override void HandleCommand(IChatClient chatClient,
            CommandReceivedEventArgs eventArgs)
        {
            string thingToBlast = eventArgs?.Arguments?.ElementAtOrDefault(0);
            switch (thingToBlast)
            {
                case "hype":
                    chatClient.SendMessage("Hype hype devchaHype !");
                    _animationDisplayNotification.Hype();
                    break;
                case "derp":
                    chatClient.SendMessage("Herp derp devchaDerp !");
                    _animationDisplayNotification.Derp();
                    break;
                default:
                    chatClient.SendMessage(HelpText);
                    break;
            }
        }
    }
}
