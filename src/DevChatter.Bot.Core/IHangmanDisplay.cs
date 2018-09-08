using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public interface IHangmanDisplay
    {
        Task HangmanStart();
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanWrongAnswer();
    }
}
