using DevChatter.Bot.Core;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class BotHub : Hub<IOverlayDisplay>
    {
        public void Ping()
        {
            Clients.All.DisplayMessage("Pong");
        }
        public void Send(string message)
        {
            Clients.All.DisplayMessage(message);
        }
        public void Hype()
        {
            Clients.All.Hype();
        }
    }
}
