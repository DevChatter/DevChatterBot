using System;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.BotModules.DuelingModule.Model
{
    public class DuelPlayed : DataEntity
    {
        public string DuelType { get; set; }
        public DateTime DateDueled { get; set; }
    }
}
