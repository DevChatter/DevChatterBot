using System.Collections.Generic;
using DevChatter.Bot.Modules.WastefulGame.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;

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

        public async Task MovePlayer(string direction, int moveNumber)
        {
            await _wastefulHubContext.Clients.All.MovePlayer(direction, moveNumber);
        }

        public async Task StartGame(string chatUserDisplayName, string chatUserUserId,
            IEnumerable<HeldItemDto> inventoryItems)
        {
            await _wastefulHubContext.Clients.All.StartGame(chatUserDisplayName, chatUserUserId, inventoryItems);
        }

        public async Task DisplaySurvivorRankings(SurvivorRankingDataDto survivorRankingData)
        {
            await _wastefulHubContext.Clients.All.DisplaySurvivorRankings(survivorRankingData);
        }
    }
}
