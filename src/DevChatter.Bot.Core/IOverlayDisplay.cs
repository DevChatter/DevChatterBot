using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public interface IOverlayDisplay
    {
        Task Hype();
        Task HangmanStart();
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanWrongAnswer();
    }
}
