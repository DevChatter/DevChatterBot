using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Streaming
{
    public interface IOverlayDisplay
    {
        Task ChangeTopic(string topic);
    }
}
