using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IHangmanDisplayNotification
    {
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanStart();
        Task HangmanWrongAnswer();
    }
}
