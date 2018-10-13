using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Streaming
{
    public interface IHangmanDisplay
    {
        Task HangmanStart(string allLetters);
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanWrongAnswer();
        Task HangmanShowGuessedLetters(string availableLetters);
    }
}
