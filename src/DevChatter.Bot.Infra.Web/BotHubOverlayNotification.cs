using DevChatter.Bot.Core;
using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.Bot.Infra.Web
{
    public class BotHubOverlayNotification : IOverlayNotification
    {
        private readonly IHubContext<BotHub, IOverlayDisplay> _botHubContext;
        private readonly IHubContext<VotingHub, IOverlayDisplay> _votingHubContext;
        private readonly IHubContext<HangmanHub, IOverlayDisplay> _hangmanHubContext;

        public BotHubOverlayNotification(IHubContext<BotHub, IOverlayDisplay> botHubContext,
            IHubContext<VotingHub, IOverlayDisplay> votingHubContext,
            IHubContext<HangmanHub, IOverlayDisplay> hangmanHubContext)
        {
            _botHubContext = botHubContext;
            _votingHubContext = votingHubContext;
            _hangmanHubContext = hangmanHubContext;
        }

        public async Task Hype()
        {
            await _botHubContext.Clients.All.Hype();
        }

        public async Task VoteStart(IEnumerable<string> choices)
        {
            await _votingHubContext.Clients.All.VoteStart(choices);
        }

        public async Task VoteReceived(ChatUser chatUser, int chosenNumber, int[] voteTotals)
        {
            var voteInfo = new VoteInfoDto
            {
                VoterChoice = chosenNumber,
                VoterName = chatUser.DisplayName,
                VoteTotals = voteTotals,
            };
            await _votingHubContext.Clients.All.VoteReceived(voteInfo);
        }

        public async Task VoteEnd()
        {
            await _votingHubContext.Clients.All.VoteEnd();
        }

        public async Task HangmanWin()
        {
            await _hangmanHubContext.Clients.All.HangmanWin();
        }

        public async Task HangmanLose()
        {
            await _hangmanHubContext.Clients.All.HangmanLose();
        }

        public async Task HangmanStart()
        {
            await _hangmanHubContext.Clients.All.HangmanStart();
        }

        public async Task HangmanWrongAnswer()
        {
            await _hangmanHubContext.Clients.All.HangmanWrongAnswer();
        }
    }
}
