using System.Collections.Generic;

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

        public bool TryPay(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
                return true;
            }

            return false;
        }
    }
}
