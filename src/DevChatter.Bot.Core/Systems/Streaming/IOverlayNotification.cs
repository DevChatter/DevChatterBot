using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IOverlayNotification
    {
        Task Hype();
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanStart();
        Task HangmanWrongAnswer();
    }
}
