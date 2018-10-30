using DevChatter.Bot.Core.BotModules.WastefulModule;
using DevChatter.Bot.Core.Systems.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.BotModules.WastefulModule.Model;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Infra.Web.Hubs
{
    public class WastefulHub : Hub<IWastefulDisplay>
    {
        private readonly IRepository _repository;
        private readonly List<IChatClient> _chatClients;

        public WastefulHub(IList<IChatClient> chatClients,
            IRepository repository)
        {
            _repository = repository;
            _chatClients = chatClients.ToList();
        }

        public void GameEnd(int points, string playerName,
            string userid, string endType, int levelNumber)
        {
            string message = $"{playerName} has {endType} on level {levelNumber} with {points} points.";
            _chatClients.ForEach(c => c.SendMessage(message));

            var gameEndRecord = new GameEndRecord
            {
                UserId = userid,
                DisplayName = playerName,
                LevelNumber = levelNumber,
                Points = points
            };

            _repository.Create(gameEndRecord);
        }
    }
}
