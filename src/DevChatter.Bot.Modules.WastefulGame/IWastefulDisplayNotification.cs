using System.Threading.Tasks;

namespace DevChatter.Bot.Modules.WastefulGame
{
    public interface IWastefulDisplayNotification
    {
        Task MovePlayer(string direction);
        Task StartGame(string chatUserDisplayName, string chatUserUserId);
    }
}
