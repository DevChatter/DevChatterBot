using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.BotModules.VotingModule;

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
        Task VoteReceived(VoteInfoDto voteInfo);
    }
}
