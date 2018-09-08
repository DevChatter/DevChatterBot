using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Streaming;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class HangmanHub : Hub<IHangmanDisplay>
    {
        public void HangmanStart()
        {
            Clients.All.HangmanStart();
        }

        public void HangmanWin()
        {
            Clients.All.HangmanWin();
        }

        public void HangmanLose()
        {
            Clients.All.HangmanLose();
        }

        public void HangmanWrongAnswer()
        {
            Clients.All.HangmanWrongAnswer();
        }
    }
}
