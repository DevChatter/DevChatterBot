using DevChatter.Bot.Core.Data.Model;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.BotModules.WastefulModule.Model
{
    public class Survivor : DataEntity
    {
        public Survivor() // Required for EF
        {
        }

        public Survivor(string displayName, string userId)
        {
        }

        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public Team Team { get; set; }
        public List<GameEndRecord> GameEndRecords { get; set; }
            = new List<GameEndRecord>();
    }
}
