using System;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.BotModules.WastefulModule.Model
{
    public class GameEndRecord : DataEntity
    {
        public Survivor Survivor { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int LevelNumber { get; set; }
        public int Points { get; set; }
    }
}
