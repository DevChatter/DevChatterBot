using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.BotModules.WastefulModule
{
    public interface IWastefulDisplayNotification
    {
        Task MovePlayer(string direction);
    }
}
