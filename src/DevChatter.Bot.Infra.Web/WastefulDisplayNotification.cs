using System.Threading.Tasks;
using DevChatter.Bot.Core.BotModules.WastefulModule;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web
{
    public class WastefulDisplayNotification : IWastefulDisplayNotification
    {
        private readonly IHubContext<WastefulHub, IWastefulDisplay> _wastefulHubContext;

        public WastefulDisplayNotification(
            IHubContext<WastefulHub, IWastefulDisplay> wastefulHubContext)
        {
            _wastefulHubContext = wastefulHubContext;
        }

        public async Task MovePlayer(string direction)
        {
            await _wastefulHubContext.Clients.All.MovePlayer(direction);
        }
    }
}
