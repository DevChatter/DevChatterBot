using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Chat
{
    public interface IKnownBotService
    {
        Task<bool> IsKnownBot(string username);
    }
}
