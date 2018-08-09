using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IOverlayNotification
    {
        Task Hype();
        Task HangmanEnd();
        Task HangmanStart();
        Task HangmanWrongAnswer();
    }
}
