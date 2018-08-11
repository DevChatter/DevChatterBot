using DevChatter.Bot.Core;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class BotHub : Hub<IOverlayDisplay>
    {
        public void Hype()
        {
            Clients.All.Hype();
        }

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
