using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.BotModules.DuelingModule.Model
{
    public class DuelPlayed : DataEntity
    {
        public string DuelType { get; set; }
        public DateTime DateDueled { get; set; }
        public List<DuelPlayerRecord> PlayerRecords { get; set; }
    }
}
