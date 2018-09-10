using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DevChatter.Bot.Infra.Web
{
    public class AnimationDisplayNotification : IAnimationDisplayNotification
    {
        private readonly IHubContext<BotHub, IAnimationDisplay> _botHubContext;

        public AnimationDisplayNotification(IHubContext<BotHub, IAnimationDisplay> botHubContext)
        {
            _botHubContext = botHubContext;
        }

        public async Task Hype()
        {
            await _botHubContext.Clients.All.Hype();
        }

        public async Task Derp()
        {
            await _botHubContext.Clients.All.Derp();
        }

    }
}
