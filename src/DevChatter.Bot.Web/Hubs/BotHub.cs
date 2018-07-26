using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Web.Hubs
{
    public class BotHub : Hub
    {
        public async Task Ping()
        {
            await Clients.All.SendAsync("PingResponse", "Pong");
        }
    }
}
