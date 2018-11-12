using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Hubs
{
    public class WastefulHub : Hub<IWastefulDisplay>
    {
        private readonly IGameRepository _repository;
        private readonly List<IChatClient> _chatClients;

        public WastefulHub(IList<IChatClient> chatClients,
            IGameRepository repository)
        {
            _repository = repository;
            _chatClients = chatClients.ToList();
        }

        public void GameEnd(int points, string playerName,
            string userId, string endType, int levelNumber, List<HeldItemDto> items)
        {
            string itemDisplayText = items.Any() ? string.Join(", ", items.Select(x => x.Name)) : "nothing";
            string message = $"{playerName} has {endType} on level {levelNumber} with {points} points while holding {itemDisplayText}.";
            _chatClients.ForEach(c => c.SendMessage(message));

            var gameEndRecord = new GameEndRecord
            {
                LevelNumber = levelNumber,
                Points = points,
                EndType = endType
            };

            Survivor survivor = _repository.Single(SurvivorPolicy.ByUserId(userId));
            if (survivor == null)
            {
                survivor = new Survivor(playerName, userId);
                survivor.GameEndRecords.Add(gameEndRecord);
                survivor.InventoryItems.AddRange(items.Select(x => x.ToInventoryItem()));
                _repository.Create(survivor);
            }
            else
            {
                survivor.GameEndRecords.Add(gameEndRecord);
                survivor.InventoryItems.AddRange(items.Select(x => x.ToInventoryItem()));
                _repository.Update(survivor);
            }
        }
    }
}
