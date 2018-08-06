using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public interface IOverlayDisplay
    {
        Task Hype();
        Task DisplayMessage(string message);
    }
}
