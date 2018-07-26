using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public interface IOverlayDisplay
    {
        Task DisplayMessage(string message);
    }
}
