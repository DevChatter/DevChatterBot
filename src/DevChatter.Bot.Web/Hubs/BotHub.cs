using DevChatter.Bot.Core;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Web.Hubs
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
    }
}
