using DevChatter.Bot.Core.Data.Model;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.BotModules.WastefulModule.Model
{
    public class Team : DataEntity
    {
        public string Name { get; set; }
        public List<Survivor> Members { get; set; }
            = new List<Survivor>();
    }
}
