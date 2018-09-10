using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core.BotModules.VotingModule;

namespace DevChatter.Bot.Core.Streaming
{
    public interface IVotingDisplay
    {
        Task VoteStart(IEnumerable<string> choices);
        Task VoteEnd();
        Task VoteReceived(VoteInfoDto voteInfo);
    }
}
