using DevChatter.Bot.Core.Data.Model;
using System.Collections.Generic;

namespace DevChatter.Bot.Modules.WastefulGame.Model
{
    public class Team : GameData
    {
        public string Name { get; set; }
        public List<Survivor> Members { get; set; }
            = new List<Survivor>();
    }
}
