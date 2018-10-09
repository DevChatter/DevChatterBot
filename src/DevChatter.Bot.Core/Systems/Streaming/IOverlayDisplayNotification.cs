using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IOverlayDisplayNotification
    {
        Task ChangeTopic(string topic);
    }
}
