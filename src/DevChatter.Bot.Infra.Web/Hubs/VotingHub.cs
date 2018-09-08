using DevChatter.Bot.Core;
using DevChatter.Bot.Core.BotModules.VotingModule;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using DevChatter.Bot.Core.Streaming;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class VotingHub : Hub<IVotingDisplay>
    {
        public void VoteStart(IEnumerable<string> choices)
        {
            Clients.All.VoteStart(choices);
        }

        public void VoteReceived(VoteInfoDto voteInfo)
        {
            Clients.All.VoteReceived(voteInfo);
        }

        public void VoteEnd()
        {
            Clients.All.VoteEnd();
        }
    }
}
