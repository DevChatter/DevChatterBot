using System;
using DevChatter.Bot.Modules.WastefulGame.Model.Enums;

namespace DevChatter.Bot.Modules.WastefulGame.Model
{
    public class GameEndRecord : GameData
    {
        public Survivor Survivor { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public int LevelNumber { get; set; }
        public int Points { get; set; }
        public EndTypes EndType { get; set; }
    }
}
