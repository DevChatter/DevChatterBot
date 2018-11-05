using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Modules.WastefulGame.GameStates
{
    public class GameStateInfo
    {
        public Survivor Survivor { get; set; }
        public List<ChatUser> Entrants { get; set; }
        public DateTime NextStartTime { get; set; }
        public string GameType { get; set; }
    }
}
