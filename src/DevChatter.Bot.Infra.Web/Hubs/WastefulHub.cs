using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.Bot.Core.BotModules.WastefulModule;
using DevChatter.Bot.Core.Systems.Chat;
using Microsoft.AspNetCore.SignalR;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class WastefulHub : Hub<IWastefulDisplay>
    {
        private readonly List<IChatClient> _chatClients;

        public WastefulHub(IList<IChatClient> chatClients)
        {
            _chatClients = chatClients.ToList();
        }

        public void GameEnd(int points, string playerName, string endType, int levelNumber)
        {
            string message = $"{playerName} has {endType} on level {levelNumber} with {points} points.";
            _chatClients.ForEach(c => c.SendMessage(message));
        }
    }
}
