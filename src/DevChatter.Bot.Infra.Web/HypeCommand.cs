using System;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web
{
    public class HypeCommand : BaseCommand
    {
        private readonly IHubContext<BotHub, IOverlayDisplay> _chatHubContext;

        public HypeCommand(IRepository repository, IHubContext<BotHub, IOverlayDisplay> chatHubContext)
            : base(repository, UserRole.Everyone)
        {
            _chatHubContext = chatHubContext;
            Cooldown = TimeSpan.FromMinutes(2);
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("Hype hype!");
            _chatHubContext.Clients.All.Hype();
        }
    }
}
