using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IHangmanDisplayNotification
    {
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanStart(string allLetters, int livesRemaining, string maskedWord);
        Task HangmanWrongAnswer();
        Task HangmanShowGuessedLetters(string availableLetters, int livesRemaining, string maskedWord);
    }
}
