using System.Threading.Tasks;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web
{
    public class BotHubOverlayNotification : IOverlayNotification
    {
        private readonly IHubContext<BotHub, IOverlayDisplay> _internalContext;

        public BotHubOverlayNotification(IHubContext<BotHub, IOverlayDisplay> internalContext)
        {
            _internalContext = internalContext;
        }

        public async Task Hype()
        {
            await _internalContext.Clients.All.Hype();
        }

        public async Task HangmanEnd()
        {
            await _internalContext.Clients.All.HangmanEnd();
        }

        public async Task HangmanStart()
        {
            await _internalContext.Clients.All.HangmanStart();
        }

        public async Task HangmanWrongAnswer()
        {
            await _internalContext.Clients.All.HangmanWrongAnswer();
        }
    }
}
