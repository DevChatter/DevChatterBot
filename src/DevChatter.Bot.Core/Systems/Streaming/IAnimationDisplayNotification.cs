using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IAnimationDisplayNotification
    {
        Task Blast(string imagePath);
    }
}
