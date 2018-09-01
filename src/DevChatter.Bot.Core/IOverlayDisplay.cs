using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core
{
    public interface IOverlayDisplay
    {
        Task Hype();
        Task VoteStart(IEnumerable<string> choices);
        Task VoteEnd();
        Task HangmanStart();
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanWrongAnswer();
    }
}
