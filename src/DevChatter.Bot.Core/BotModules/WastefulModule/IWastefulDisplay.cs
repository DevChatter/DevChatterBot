using System.Threading.Tasks;

namespace DevChatter.Bot.Core.BotModules.WastefulModule
{
    public interface IWastefulDisplay
    {
        Task MovePlayer(string direction);
        Task StartGame(string chatUserDisplayName, string chatUserUserId);
    }
}
