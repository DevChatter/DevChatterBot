using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IHangmanDisplayNotification
    {
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanStart(string allLetters);
        Task HangmanWrongAnswer();
        Task HangmanShowGuessedLetters(string availableLetters);
    }
}
