using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.BotModules.VotingModule;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web
{
    public class BotHubOverlayNotification : IOverlayNotification
    {
        private readonly IHubContext<BotHub, IOverlayDisplay> _internalContext;

        public BotHubOverlayNotification(IHubContext<BotHub, IOverlayDisplay> internalContext)
        {
            _internalContext = internalContext;
        }

        public async Task Hype()
        {
            await _internalContext.Clients.All.Hype();
        }

        public async Task VoteStart(IEnumerable<string> choices)
        {
            await _internalContext.Clients.All.VoteStart(choices);
        }

        public async Task VoteReceived(ChatUser chatUser, int chosenNumber, int[] voteTotals)
        {
            var voteInfo = new VoteInfoDto
            {
                VoterChoice = chosenNumber,
                VoterName = chatUser.DisplayName,
                VoteTotals = voteTotals,
            };
            await _internalContext.Clients.All.VoteReceived(voteInfo);
        }

        public async Task VoteEnd()
        {
            await _internalContext.Clients.All.VoteEnd();
        }

        public async Task HangmanWin()
        {
            await _internalContext.Clients.All.HangmanWin();
        }

        public async Task HangmanLose()
        {
            await _internalContext.Clients.All.HangmanLose();
        }

        public async Task HangmanStart()
        {
            await _internalContext.Clients.All.HangmanStart();
        }

        public async Task HangmanWrongAnswer()
        {
            await _internalContext.Clients.All.HangmanWrongAnswer();
        }
    }
}
