using System.Threading.Tasks;
using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web
{
    public class OverlayDisplayNotification : IOverlayDisplayNotification
    {
        private readonly IHubContext<OverlayHub, IOverlayDisplay> _botHubContext;

        public OverlayDisplayNotification(IHubContext<OverlayHub, IOverlayDisplay> botHubContext)
        {
            _botHubContext = botHubContext;
        }

        public async Task ChangeTopic(string topic)
        {
            await _botHubContext.Clients.All.ChangeTopic(topic);
        }
    }
}