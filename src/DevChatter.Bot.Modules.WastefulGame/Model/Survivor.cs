using System;
using DevChatter.Bot.Modules.WastefulGame.Model.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Model
{
    public class Survivor : GameData
    {
        public Survivor() // Required for EF
        {
        }

        public Survivor(string displayName, string userId)
        {
            DisplayName = displayName;
            UserId = userId;
        }

        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public Team Team { get; set; }
        public long Money { get; set; } = 100;
        public List<GameEndRecord> GameEndRecords { get; set; }
            = new List<GameEndRecord>();

        public List<InventoryItem> InventoryItems { get; set; }
            = new List<InventoryItem>();

        public Location Location { get; set; }

        public bool TryPay(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
                return true;
            }

            return false;
        }

        public void ApplyEndGame(int levelNumber, int points,
            EndTypes endType, IList<InventoryItem> items, Location location)
        {
            var gameEndRecord = new GameEndRecord
            {
                LevelNumber = levelNumber,
                Points = points,
                EndType = endType
            };

            GameEndRecords.Add(gameEndRecord);
            InventoryItems.Clear();
            if (endType != EndTypes.Died)
            {
                Location = location;
                if (items.Any())
                {
                    InventoryItems.AddRange(items);
                }
            }
        }

        public bool SellItem(int inventoryIndex)
        {
            InventoryItem item = InventoryItems.ElementAtOrDefault(inventoryIndex);
            if (item == null)
            {
                return false;
            }

            InventoryItems.RemoveAt(inventoryIndex);
            Money += 25;

            return true;
        }

        public bool BuyItem(ShopItem shopItem)
        {
            if (TryPay(shopItem.Price))
            {
                InventoryItems.Add(new InventoryItem(shopItem));
                return true;
            }
            return false;
        }

        public bool LeaveTeam()
        {
            if (Team == null)
            {
                return false;
            }

            Team = null;
            return true;
        }
    }
}
