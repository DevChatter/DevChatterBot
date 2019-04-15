using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Streaming;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Infra.Web
{
    public class VotingDisplayNotification : IVotingDisplayNotification
    {
        private readonly IHubContext<VotingHub, IVotingDisplay> _votingHubContext;

        public VotingDisplayNotification(
            IHubContext<VotingHub, IVotingDisplay> votingHubContext)
        {
            _votingHubContext = votingHubContext;
        }

        public async Task VoteStart(IEnumerable<string> choices)
        {
            await _votingHubContext.Clients.All.VoteStart(choices);
        }

        public async Task VoteReceived(ChatUser chatUser, string chosenName, int[] voteTotals)
        {
            var voteInfo = new VoteInfoDto
            {
                VoterChoiceName = chosenName,
                VoterName = chatUser.DisplayName,
                VoteTotals = voteTotals,
            };
            await _votingHubContext.Clients.All.VoteReceived(voteInfo);
        }

        public async Task VoteEnd()
        {
            await _votingHubContext.Clients.All.VoteEnd();
        }

    }
}
