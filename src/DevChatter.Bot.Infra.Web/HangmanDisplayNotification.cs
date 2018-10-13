using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Streaming;

namespace DevChatter.Bot.Infra.Web
{
    public class HangmanDisplayNotification : IHangmanDisplayNotification
    {
        private readonly IHubContext<HangmanHub, IHangmanDisplay> _hangmanHubContext;

        public HangmanDisplayNotification(
            IHubContext<HangmanHub, IHangmanDisplay> hangmanHubContext)
        {
            _hangmanHubContext = hangmanHubContext;
        }

        public async Task HangmanWin()
        {
            await _hangmanHubContext.Clients.All.HangmanWin();
        }

        public async Task HangmanLose()
        {
            await _hangmanHubContext.Clients.All.HangmanLose();
        }

        public async Task HangmanStart(string allLetters)
        {
            await _hangmanHubContext.Clients.All.HangmanStart(allLetters);
        }

        public async Task HangmanWrongAnswer()
        {
            await _hangmanHubContext.Clients.All.HangmanWrongAnswer();
        }

        public async Task HangmanShowGuessedLetters(string availableLetters)
        {
            await _hangmanHubContext.Clients.All.HangmanShowGuessedLetters(availableLetters);
        }
    }
}
