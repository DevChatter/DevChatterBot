using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Streaming
{
    public interface IAnimationDisplay
    {
        Task Blast(string imagePath);
        Task Fireworks();
    }
}
