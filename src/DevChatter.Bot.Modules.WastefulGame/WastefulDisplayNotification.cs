using DevChatter.Bot.Modules.WastefulGame.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DevChatter.Bot.Modules.WastefulGame
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

        public async Task StartGame(string chatUserDisplayName, string chatUserUserId)
        {
            await _wastefulHubContext.Clients.All.StartGame(chatUserDisplayName, chatUserUserId);
        }
    }
}
