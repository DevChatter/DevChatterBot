using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IOverlayNotification
    {
        Task Hype();
        Task VoteStart(IEnumerable<string> choices);
        Task VoteEnd();
        Task HangmanWin();
        Task HangmanLose();
        Task HangmanStart();
        Task HangmanWrongAnswer();
        Task VoteReceived(ChatUser chatUser, int chosenNumber, int[] voteTotals);
        Task Derp();
    }
}
