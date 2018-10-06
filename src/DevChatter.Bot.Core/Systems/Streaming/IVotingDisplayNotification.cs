using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Systems.Streaming
{
    public interface IVotingDisplayNotification
    {
        Task VoteStart(IEnumerable<string> choices);
        Task VoteEnd();
        Task VoteReceived(ChatUser chatUser, string chosenName, int[] voteTotals);
    }
}
