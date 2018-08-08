using DevChatter.Bot.Core;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class BotHub : Hub<IOverlayDisplay>
    {
        public void Hype()
        {
            Clients.All.Hype();
        }
    }
}
