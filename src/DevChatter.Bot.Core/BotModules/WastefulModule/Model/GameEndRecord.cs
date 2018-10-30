using System;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.BotModules.WastefulModule.Model
{
    public class GameEndRecord : DataEntity
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int LevelNumber { get; set; }
        public int Points { get; set; }
    }
}
