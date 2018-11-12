using System;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;
using DevChatter.Bot.Modules.WastefulGame.Model;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Modules.WastefulGame.Model.Enums;

namespace DevChatter.Bot.Modules.WastefulGame.Hubs
{
    public class WastefulHub : Hub<IWastefulDisplay>
    {
        private readonly SurvivorRepo _survivorRepo;
        private readonly List<IChatClient> _chatClients;

        public WastefulHub(IList<IChatClient> chatClients,
            SurvivorRepo survivorRepo)
        {
            _survivorRepo = survivorRepo;
            _chatClients = chatClients.ToList();
        }

        public void GameEnd(int points, string playerName,
            string userId, EndTypes endType, int levelNumber, List<HeldItemDto> items)
        {
            string itemDisplayText = items.Any()
                ? string.Join(", ", items.Select(x => x.Name))
                : "nothing";
            string message = $"{playerName} has {endType} on level {levelNumber} with {points} points while holding {itemDisplayText}.";
            _chatClients.ForEach(c => c.SendMessage(message));

            Survivor survivor = _survivorRepo.GetOrCreate(playerName, userId);

            var inventoryItems = items.Select(x => x.ToInventoryItem()).ToList();
            survivor.ApplyEndGame(levelNumber, points, endType, inventoryItems);

            _survivorRepo.Save(survivor);
        }
    }
}
