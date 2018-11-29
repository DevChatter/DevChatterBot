using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;

namespace DevChatter.Bot.Modules.WastefulGame
{
    public interface IWastefulDisplay
    {
        Task MovePlayer(string direction, int moveNumber);
        Task StartGame(string chatUserDisplayName, string chatUserUserId, IEnumerable<HeldItemDto> inventoryItems);
        Task DisplaySurvivorRankings(SurvivorRankingDataDto survivorRankingData);
    }
}
