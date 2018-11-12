using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;
using DevChatter.Bot.Modules.WastefulGame.Model.Enums;

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
        public long Money { get; set; }
        public List<GameEndRecord> GameEndRecords { get; set; }
            = new List<GameEndRecord>();

        public List<InventoryItem> InventoryItems { get; set; }
            = new List<InventoryItem>();

        public bool TryPay(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
                return true;
            }

            return false;
        }

        public void ApplyEscape(int levelNumber, int points, IEnumerable<InventoryItem> items)
        {
            var gameEndRecord = new GameEndRecord
            {
                LevelNumber = levelNumber,
                Points = points,
                EndType = EndTypes.Escaped
            };

            GameEndRecords.Add(gameEndRecord);
            InventoryItems.Clear();
            InventoryItems.AddRange(items);
        }
    }
}
