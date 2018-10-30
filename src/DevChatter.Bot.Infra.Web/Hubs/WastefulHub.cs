using DevChatter.Bot.Core.BotModules.WastefulModule;
using DevChatter.Bot.Core.BotModules.WastefulModule.Model;
using DevChatter.Bot.Core.BotModules.WastefulModule.Model.Specifications;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Systems.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;

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
            string userId, string endType, int levelNumber)
        {
            string message = $"{playerName} has {endType} on level {levelNumber} with {points} points.";
            _chatClients.ForEach(c => c.SendMessage(message));

            var gameEndRecord = new GameEndRecord
            {
                LevelNumber = levelNumber,
                Points = points
            };

            Survivor survivor = _repository.Single(SurvivorPolicy.ByUserId(userId));
            if (survivor == null)
            {
                survivor = new Survivor(playerName, userId);
                survivor.GameEndRecords.Add(gameEndRecord);
                _repository.Create(survivor);
            }
            else
            {
                gameEndRecord.Survivor = survivor;
                _repository.Create(gameEndRecord);
            }
        }
    }
}
